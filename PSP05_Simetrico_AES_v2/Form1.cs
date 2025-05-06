using System;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Remoting;

namespace PSP05_Simetrico_AES
{
    public partial class Form1 : Form
    {
        private Aes objAES = null;
        private byte[] bytextoCifrado;

        public Form1()
        {
             
                InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                
                string textoMensaje = this.textBox1.Text;
                this.objAES = Aes.Create();
                bytextoCifrado = EncriptarTextoAMemoria(textoMensaje, this.objAES);
                this.label4.Text = BitConverter.ToString(bytextoCifrado).ToLower().Replace("-","");
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string textoDescifrado;
                
                textoDescifrado = DesencriptarMemoriaATexto(this.bytextoCifrado, this.objAES);
                this.label2.Text = textoDescifrado;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }

        }

        //Método encriptar texto a Fichero
        //@data = Texto a cifrar
        //@Aesalg = Objeto de la clase Aes, que contiene la clave y el vector de inicialización.
        static byte[] EncriptarTextoAMemoria(String Data, Aes Aesalg)
        {
            byte[] encriptado_bytes;

                ICryptoTransform encryptor = Aesalg.CreateEncryptor(Aesalg.Key, Aesalg.IV);

                //En vez de un fichero vamos a hacer uso de la memoria para guardar los datos.

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(cStream))
                        {
                            swEncrypt.Write(Data);
                        }
                        encriptado_bytes = msEncrypt.ToArray();
                    }
                }
                return encriptado_bytes;
        }
       
        static string DesencriptarMemoriaATexto(byte[] cipherText, Aes aesAlg)
        {
            string textoplano = null;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            textoplano = srDecrypt.ReadToEnd();
                        }
                    }
                }
            return textoplano;
        }
        

       
    }
}
