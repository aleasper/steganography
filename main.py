import argparse
import core.encrypt_decrypt as crypto
import sys
import os

from consts.consts import WAV_INFO_BYTES
from core.mask import create_masks


def get_available_size(wav_data_len, degree):
    return wav_data_len * degree / 16


# def create_masks(degree):
#     """
#     Create masks for taking bits from text bytes and
#     putting them to image bytes.
#     :param degree: number of bits from byte that are taken to encode text data in audio
#     :return:  mask for a text and a mask for a sample
#     """
#     text_mask = 0b1111111111111111
#     sample_mask = 0b1111111111111111
#
#     text_mask <<= (16 - degree)
#     text_mask %= 65536
#     sample_mask >>= degree
#     sample_mask <<= degree
#
#     return text_mask, sample_mask

# --input input.wav --output output1.wav --text_file text.txt
if __name__ == '__main__':
    parser = argparse.ArgumentParser()
    parser.add_argument("--input", type=str,
                        help="input file")
    parser.add_argument("--text_file", type=str,
                        help="file with text to steganography")
    parser.add_argument("--output", type=str,
                        help="output file", nargs='?', default=None)
    parser.add_argument("--degree", type=int,
                        help="amount of bytes to overwrite", nargs='?', default=2)
    parser.add_argument('--encode', dest='encode', action='store_true')
    args = parser.parse_args()
    input_file_path = args.input
    text_file_path = args.text_file
    ENCODING_BITS = args.degree
    print(f'using {ENCODING_BITS} bits to overwrite')
    src_file = open(input_file_path, 'rb')

    file_extension = os.path.splitext(input_file_path)[1]
    encoded_bytes_amount = 16

    if file_extension == '.wav':
        pass
    elif file_extension == '.bmp':
        encoded_bytes_amount = 8
        pass
    else:
        print('Unsupported file extension')
        sys.exit(1)

    if args.encode:
        text_file_path = args.text_file
        output_file_path = args.output
        dst_file = open(output_file_path, 'wb')
        text = open(text_file_path, 'r')
        text_len = os.stat(text_file_path).st_size

        wav_info = src_file.read(WAV_INFO_BYTES)
        wav_data_len = int.from_bytes(wav_info[40:44], byteorder='little')

        available_size = get_available_size(wav_data_len, ENCODING_BITS)
        if text_len > available_size:
            print("Source file is too small to encode this text")
            sys.exit(1)

        dst_file.write(wav_info)

        wav_data = src_file.read(wav_data_len)

        text_mask, sample_mask = create_masks(ENCODING_BITS, encoded_bytes_amount)
        text_str = text.read()
        text_arr = [char for char in text_str]
        print(text_arr)
        res = crypto.encrypt(text_arr,
                             ENCODING_BITS,
                             wav_data,
                             src_file.read(),
                             encoded_bytes_amount,
                             text_mask, sample_mask)
        dst_file.write(res)
        # end_symbol_added = False
        #
        # while True:
        #     txt_symbol = text.read(1)
        #
        #     # text is ended
        #     if not txt_symbol:
        #         if not end_symbol_added:
        #             end_symbol_added = True
        #             txt_symbol = '~'
        #         else:
        #             break
        #
        #     txt_symbol = ord(txt_symbol)  # from char to ASCII integer
        #     # txt_symbol <<= 8
        #
        #     for step in range(0, 16, ENCODING_BITS):
        #         if step == 8 and not txt_symbol:
        #             break
        #
        #         sample = int.from_bytes(wav_data[:2], byteorder='little') & sample_mask
        #         wav_data = wav_data[2:]
        #
        #         bits = txt_symbol & text_mask
        #         bits >>= (16 - ENCODING_BITS)
        #
        #         sample |= bits
        #
        #         dst_file.write(sample.to_bytes(2, byteorder='little'))
        #         txt_symbol = (txt_symbol << ENCODING_BITS) % 65536
        #
        #
        # dst_file.write(wav_data)
        # dst_file.write(src_file.read())

        text.close()
        src_file.close()
        dst_file.close()
    else:

        ''' DECODE '''

        input_wav = open(input_file_path, 'rb')
        text = open(text_file_path, 'w')

        wav_header = input_wav.read(WAV_INFO_BYTES)
        data_size = int.from_bytes(wav_header[40:44], byteorder='little')

        _, sample_mask = create_masks(ENCODING_BITS)
        sample_mask = ~sample_mask

        data = input_wav.read(data_size)

        decoded_text = ''

        read = 0

        end_symbol_read = False

        while not end_symbol_read:
            two_symbols = 0
            for step in range(0, 16, ENCODING_BITS):
                sample = int.from_bytes(data[:2], byteorder='little') & sample_mask
                data = data[2:]

                two_symbols <<= ENCODING_BITS
                two_symbols |= sample

            # first_symbol = two_symbols >> 8
            first_symbol = two_symbols

            symbol = chr(first_symbol)
            if symbol == '~':
                end_symbol_read = True
            else:
                decoded_text += symbol
            read += 1

            if chr(first_symbol) == '\n' and len(os.linesep) == 2:
                read += 1

            # if data_size - read > 0 and not end_symbol_read:
            #     second_symbol = two_symbols & 0b0000000011111111
            #     symbol = (chr(second_symbol))
            #     if symbol == '~':
            #         end_symbol_read = True
            #     else:
            #         decoded_text += symbol
            #     read += 1
            #
            #     if chr(second_symbol) == '\n' and len(os.linesep) == 2:
            #         read += 1

        text.write(decoded_text)
        text.close()
        input_wav.close()
    sys.exit(0)
