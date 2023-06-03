using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ErkeSmx
{
    internal class LoginR
    {
        public static string RSAPublicKey(string publicKey)
        {
            RsaKeyParameters publicKeyParam = (RsaKeyParameters)PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey));
            string XML = string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>",
            Convert.ToBase64String(publicKeyParam.Modulus.ToByteArrayUnsigned()),
            Convert.ToBase64String(publicKeyParam.Exponent.ToByteArrayUnsigned()));
            return XML;
        }

        public static string RSAEncrypt(string publickey, string content)
        {
            RSACryptoServiceProvider _rsa = new RSACryptoServiceProvider();
            _rsa.FromXmlString(publickey);
            var _b = Encoding.Default.GetBytes(content);
            _b = Encoding.Convert(Encoding.Default, new UTF8Encoding(), _b);
            int _maxSize = _rsa.KeySize / 8 - 11;
            if (_b.Length <= _maxSize)
            {
                byte[] cipherbytes = _rsa.Encrypt(_b, false);
                return Convert.ToBase64String(cipherbytes);
            }
            else
            {
                using (MemoryStream PlaiStream = new MemoryStream(_b))
                using (MemoryStream CrypStream = new MemoryStream())
                {
                    Byte[] Buffer = new Byte[_maxSize];
                    int BlockSize = PlaiStream.Read(Buffer, 0, _maxSize);
                    while (BlockSize > 0)
                    {
                        Byte[] ToEncrypt = new Byte[BlockSize];
                        Array.Copy(Buffer, 0, ToEncrypt, 0, BlockSize);
                        Byte[] Cryptograph = _rsa.Encrypt(ToEncrypt, false);
                        CrypStream.Write(Cryptograph, 0, Cryptograph.Length);
                        BlockSize = PlaiStream.Read(Buffer, 0, _maxSize);
                    }
                    return Convert.ToBase64String(CrypStream.ToArray(), Base64FormattingOptions.None);
                }
            }
        }
    }
}
