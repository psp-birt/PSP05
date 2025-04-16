using System;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.IO;

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
                bytextoCifrado = EncriptarTextoAMemoria(textoMensaje, this.objAES.Key, this.objAES.IV);
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
                
                textoDescifrado = DesencriptarMemoriaATexto(this.bytextoCifrado, objAES.Key, objAES.IV);
                this.label2.Text = textoDescifrado;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }

        }

        //Método encriptar texto a Fichero
        //@data = Texto a cifrar
        //@Key = Clave generada
        //@IV = Vector generado
        static byte[] EncriptarTextoAMemoria(String Data, byte[] Key, byte[] IV)
        {
            byte[] encriptado_bytes;
            using (Aes Aesalg = Aes.Create())
            {

                //Creamos un objeto de tipo AES para asignarle el key y el vector que utilizaremos
                
                Aesalg.Key = Key;
                Aesalg.IV = IV;

                //Crea un objeto de CryptoStream. Este objeto sirve para unir el flujo común de datos con transformaciones criptográficas.
                //Pasamos como parámetro.
                //@msEncrypt = El flujo en el que se vuelca la transformación criptográfica.
                //@DESalg.CreateEncryptor(Key, IV)  = Modalidad de transformación criptográfica. Algoritmo AES, con clave y vector.
                //@CryptoStreamMode.Write = Modo en el que se realizará la criptografía- En este caso escritura.
              

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
            
        }
       
        static string DesencriptarMemoriaATexto(byte[] cipherText, byte[] Key, byte[] IV)
        {
            string textoplano = null;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
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
            }
            return textoplano;
        }
        

       
    }
}
