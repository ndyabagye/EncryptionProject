using System.Security.Cryptography;

static byte[] Encrypt(string simpletext, byte[] key, byte[] iv)
{
    byte[] cipheredText;
    using (Aes aes = Aes.Create())
    {
        ICryptoTransform encryptor = aes.CreateEncryptor(key, iv);
        using (MemoryStream memoryStream = new MemoryStream())
        {
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                {
                    streamWriter.Write(simpletext);
                }

                cipheredText = memoryStream.ToArray();
            }
        }
    }
    return cipheredText;
}

static string Decrypt(byte[] cipheredText, byte[] key, byte[]iv)
{
    string simpletext = String.Empty;
    using (Aes aes = Aes.Create())
    {
        ICryptoTransform decryptor = aes.CreateDecryptor(key, iv);
        using (MemoryStream memoryStream = new MemoryStream(cipheredText))
        {
            using(CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            {
                using (StreamReader streamReader = new StreamReader(cryptoStream))
                {
                    simpletext = streamReader.ReadToEnd();
                }
            }
        }
    }
    return simpletext;
}

//static void Main(string[] args)
//{
    Console.WriteLine("Please enter your username");
    string username = Console.ReadLine();

    Console.WriteLine("Please enter a password");
    string password = Console.ReadLine();

    // Generate the key and IV
    byte[] key = new byte[16];

    byte[] iv = new byte[16];

    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
    {
        rng.GetBytes(key);
    }

    //Encrypt the password
    byte[] encryptedPassword = Encrypt(password, key, iv);
    string encryptedPasswordString = Convert.ToBase64String(encryptedPassword);
    Console.WriteLine($"Encrypted Password: {encryptedPasswordString}");

    //Decrypt the password
    string decryptedPassword = Decrypt(encryptedPassword, key, iv);
    Console.WriteLine($"Decrypted Password: {decryptedPassword}");
    Console.ReadLine();
//}