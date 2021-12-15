def create_masks(degree, encoded_bytes_amount):
    """
    Create masks for taking bits from text bytes and
    putting them to file bytes.
    :param degree: number of bits from byte that are taken to encode text data in audio
    :param encoded_bytes_amount: amount of sample bites
    :return: mask for a text and a mask for a sample
    """
    if encoded_bytes_amount == 16:
        text_mask = 0b1111111111111111
        sample_mask = 0b1111111111111111
    else:
        text_mask = 0b11111111
        sample_mask = 0b11111111

    text_mask <<= (encoded_bytes_amount - degree)
    if encoded_bytes_amount == 16:
        text_mask %= 65536
    else:
        text_mask %= 256

    sample_mask >>= degree
    sample_mask <<= degree

    return text_mask, sample_mask
