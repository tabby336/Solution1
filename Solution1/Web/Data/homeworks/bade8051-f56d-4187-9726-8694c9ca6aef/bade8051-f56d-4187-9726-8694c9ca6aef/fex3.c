#include <openssl/aes.h>

#include <openssl/evp.h>

#include <string>

#include <iostream>

#include <fstream>

using namespace std;

string iv = "BBBBBBBBBBBBBBBB";

int blockSize(EVP_CIPHER_CTX ctx_)

{

	return EVP_CIPHER_CTX_block_size(&ctx_);

}

string aes_decrypt(string input, string key, string mode)
{
	/* decription method, using mostly c++ only, long live strings. Receives input, key, encryption mode, returns decryption result. The steps are pretty much standard,
	taken directly out of the openssl documentation, but with a c++ twist */
	string output;

	EVP_CIPHER_CTX e_ctx;

	const EVP_CIPHER *cryptoAlgorithm;

	EVP_CIPHER_CTX_init(&e_ctx);

	if (mode == "ECB")

	{

		cryptoAlgorithm = EVP_aes_128_ecb();

		EVP_DecryptInit_ex(&e_ctx, cryptoAlgorithm, NULL,

			(const unsigned char*)(key.data()),

			NULL);

	}

	else {

		cryptoAlgorithm = EVP_aes_128_cfb();

		EVP_DecryptInit_ex(&e_ctx, cryptoAlgorithm, NULL,

			(const unsigned char*)(key.data()),

			(const unsigned char*)(iv.data()));

	}