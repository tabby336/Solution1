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

	EVP_CIPHER_CTX_set_padding(&e_ctx, 1);

	//std::cout << blockSize(e_ctx) << '\n';

	output.resize(input.size() + blockSize(e_ctx));

	int resultLength, finale;

	EVP_DecryptUpdate(&e_ctx,

		(unsigned char*)output.data(),

		&resultLength,

		(unsigned char*)input.data(),

		input.size());

	//std::cout << output << '\n';
	//std::cout << output.size() << '\n';
	//  output.resize(resultLength);
	//  std::cout << resultLength << '\n';

	//  output.resize(blockSize(e_ctx) + resultLength);

	EVP_DecryptFinal_ex(&e_ctx,

		(unsigned char*)(output.data() + resultLength),

		&finale);
	//  std::cout << finale << '\n';
	//std::cout << "debug2" << '\n';
	//std::cout << output << '\n';
	output.resize(resultLength + finale); // for ECB purpouse
										  //std::cout << output << '\n';
										  //  std::cout << resultLength + finale << '\n';

	return output;

}


string process_string(string key_)
{
	/* Make each key 16 bytes long, avoid nasty hex conversion */

	//std::cout << key_.size() << '\n';
	while (key_.size() < 16) {
		key_.append(" ");
	}
	//std::cout << key_.size() << '\n';
	return key_;
}

string read_file(string filename)
{
	/* helper function to read an whole file using an iterator and puting everything into a nice string. Probably a bad idea to use this if the file is large */

	ifstream t(filename.c_str());

	string str((istreambuf_iterator<char>(t)),

		istreambuf_iterator<char>());

	return str;

}

string get_key(string pt, string ct, string mode)
{
	/* Core function, exaustively searches for the key, in the given document. Returns the key and prints the number of tries until the key is found/not found */

	string expected_output = read_file(pt); // Read plain text, compare each decription try with this
	string cipher_text = read_file(ct); //read cipher text. We will attemt to break this puppy
	string key;

	ifstream fin("word_dict.txt");

	int nr_of_tries = 0;

	while (fin >> key) {
		string output = aes_decrypt(cipher_text, process_string(key), mode);
		if (output == expected_output) {
			cout << "Number of keys attemted until the right one was found: " << nr_of_tries << '\n';
			return key;
		}
		++nr_of_tries;
	}
	fin.close();
	cout << nr_of_tries << '\n';
	return "no key was found";
}

int main()
{
	/*
	string input = read_file("plain.txt");
	//string key = "AAAAAAAAAAAAAAAA";
	string key2 = process_string("median");
	string ecb_file = read_file("enc_ecb.txt");
	string cfb_file = read_file("enc_cfb.txt");
	string plain_ecb = aes_decrypt(ecb_file, key2, "ECB");
	string plain_cfb = aes_decrypt(cfb_file, key2, "CFB");
	std::cout << plain_ecb << '\n';
	std::cout << plain_cfb << '\n';
	*/
	cout << "Key used for ECB encryption: " << get_key("plain.txt", "enc_ecb.txt", "ECB") << '\n';
	cout << "Key used for CFB encryption: " << get_key("plain.txt", "enc_cfb.txt", "CFB") << '\n';
	return 0;
}

/* Assertions for magic numbers. */

#define ASSERT_IS_ENUM_DESCRIPTOR(desc) \
	assert((desc)->magic == PROTOBUF_C__ENUM_DESCRIPTOR_MAGIC)

#define ASSERT_IS_MESSAGE_DESCRIPTOR(desc) \
	assert((desc)->magic == PROTOBUF_C__MESSAGE_DESCRIPTOR_MAGIC)

#define ASSERT_IS_MESSAGE(message) \
	ASSERT_IS_MESSAGE_DESCRIPTOR((message)->descriptor)

#define ASSERT_IS_SERVICE_DESCRIPTOR(desc) \
	assert((desc)->magic == PROTOBUF_C__SERVICE_DESCRIPTOR_MAGIC)

/**@}*/

/* --- version --- */

const char *
protobuf_c_version(void)
{
	return PROTOBUF_C_VERSION;
}

uint32_t
protobuf_c_version_number(void)
{
	return PROTOBUF_C_VERSION_NUMBER;
}

/* --- allocator --- */

static void *
system_alloc(void *allocator_data, size_t size)
{
	return malloc(size);
}

static void
system_free(void *allocator_data, void *data)
{
	free(data);
}

static inline void *
do_alloc(ProtobufCAllocator *allocator, size_t size)
{
	return allocator->alloc(allocator->allocator_data, size);
}

static inline void
do_free(ProtobufCAllocator *allocator, void *data)
{
	if (data != NULL)
		allocator->free(allocator->allocator_data, data);
}

/*
* This allocator uses the system's malloc() and free(). It is the default
* allocator used if NULL is passed as the ProtobufCAllocator to an exported
* function.
*/
static ProtobufCAllocator protobuf_c__allocator = {
	.alloc = &system_alloc,
	.free = &system_free,
	.allocator_data = NULL,
};

/* === buffer-simple === */

void
protobuf_c_buffer_simple_append(ProtobufCBuffer *buffer,
	size_t len, const uint8_t *data)
{
	ProtobufCBufferSimple *simp = (ProtobufCBufferSimple *)buffer;
	size_t new_len = simp->len + len;

	if (new_len > simp->alloced) {
		ProtobufCAllocator *allocator = simp->allocator;
		size_t new_alloced = simp->alloced * 2;
		uint8_t *new_data;

		if (allocator == NULL)
			allocator = &protobuf_c__allocator;
		while (new_alloced < new_len)
			new_alloced += new_alloced;
		new_data = do_alloc(allocator, new_alloced);
		if (!new_data)
			return;
		memcpy(new_data, simp->data, simp->len);
		if (simp->must_free_data)
			do_free(allocator, simp->data);
		else
			simp->must_free_data = TRUE;
		simp->data = new_data;
		simp->alloced = new_alloced;
	}
	memcpy(simp->data + simp->len, data, len);
	simp->len = new_len;
}

/**
* \defgroup packedsz protobuf_c_message_get_packed_size() implementation
*
* Routines mainly used by protobuf_c_message_get_packed_size().
*
* \ingroup internal
* @{
*/

/**
* Return the number of bytes required to store the tag for the field. Includes
* 3 bits for the wire-type, and a single bit that denotes the end-of-tag.
*
* \param number
*      Field tag to encode.
* \return
*      Number of bytes required.
*/
static inline size_t
get_tag_size(uint32_t number)
{
	if (number < (1UL << 4)) {
		return 1;
	}
	else if (number < (1UL << 11)) {
		return 2;
	}
	else if (number < (1UL << 18)) {
		return 3;
	}
	else if (number < (1UL << 25)) {
		return 4;
	}
	else {
		return 5;
	}
}

/**
* Return the number of bytes required to store a variable-length unsigned
* 32-bit integer in base-128 varint encoding.
*
* \param v
*      Value to encode.
* \return
*      Number of bytes required.
*/
static inline size_t
uint32_size(uint32_t v)
{
	if (v < (1UL << 7)) {
		return 1;
	}
	else if (v < (1UL << 14)) {
		return 2;
	}
	else if (v < (1UL << 21)) {
		return 3;
	}
	else if (v < (1UL << 28)) {
		return 4;
	}
	else {
		return 5;
	}
}