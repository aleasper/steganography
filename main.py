import argparse
import core.encrypt_decrypt as crypto

if __name__ == '__main__':
    parser = argparse.ArgumentParser()
    parser.add_argument("input", type=str,
                        help="input file")
    parser.add_argument("text_file", type=str,
                        help="file with text to steganography")
    parser.add_argument("output", type=str,
                        help="output file")
    args = parser.parse_args()
    print(args)
    crypto.encrypt()
