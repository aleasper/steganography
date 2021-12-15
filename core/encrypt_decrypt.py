


def encrypt(text, ENCODING_BITS, file_data, file_end, encoded_bytes_amount, text_mask, sample_mask):
    end_symbol_added = False
    res = bytearray(b'')
    while True:
        try:
            txt_symbol = text.pop(0)
        except:
            break
        # text is ended
        if not txt_symbol:
            if not end_symbol_added:
                end_symbol_added = True
                txt_symbol = '~'
            else:
                break

        txt_symbol = ord(txt_symbol)  # from char to ASCII integer
        # txt_symbol <<= 8

        for step in range(0, encoded_bytes_amount, ENCODING_BITS):
            # if step == 8 and not txt_symbol:
            #     break

            sample = int.from_bytes(file_data[:int(encoded_bytes_amount/8)], byteorder='little') & sample_mask
            file_data = file_data[int(encoded_bytes_amount/8):]

            bits = txt_symbol & text_mask
            bits >>= (encoded_bytes_amount - ENCODING_BITS)

            sample |= bits
            t = bytearray(sample.to_bytes(int(encoded_bytes_amount/8), byteorder='little'))
            res += t
            sample_size = 65536
            if encoded_bytes_amount == 8:
                sample_size = 256

            txt_symbol = (txt_symbol << ENCODING_BITS) % sample_size

    res += bytearray(file_data)
    res += bytearray(file_end)
    return res