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

                //Creamos el objeto objAES para conseguir la clave única para cifrar y descifrar
                //al crear un objeto AES se genera la key y el vector IV
                //Se crea un objeto criptográfico objAES

                this.objAES = Aes.Create();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Try-catch ya que trabajaremos con ficheros en método encriptar
            try
            {
                
                string textoMensaje = this.textBox1.Text;

                //Llamada a método encriptar ( en este caso a memoria)
                bytextoCifrado = EncriptarTextoAMemoria(textoMensaje, this.objAES);

                //Mostramos el texto cifrado en hexadecimal
                this.label4.Text = BitConverter.ToString(bytextoCifrado).ToLower().Replace("-","");
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Try-catch ya que trabajaremos con ficheros en método desencriptar
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

        //Método encriptar texto a Memoria
        //@data = Texto a cifrar
        //@Aesalg = Objeto de la clase Aes, que contiene la clave y el vector de inicialización.
        static byte[] EncriptarTextoAMemoria(String Data, Aes Aesalg)
        {
            byte[] encriptado_bytes;

            //Creamos el objeto de cifrado AES
            ICryptoTransform encryptor = Aesalg.CreateEncryptor(Aesalg.Key, Aesalg.IV);

            //En vez de un fichero vamos a hacer uso de la memoria para guardar los datos. Haciendo uso de la clase MemoryStream.
            //En vez de un try-catch , se hace un using para que al finalizar el bloque de código se cierre el stream y no haya fugas de memoria.
            using (MemoryStream msEncrypt = new MemoryStream())
            {

                //Crea el flujo de cifrado:esta línea de código establece la conexión entre el flujo de memoria y el algoritmo de cifrado AES, permitiendo que los datos se cifren y se escriban en la memoria.
                //@msEncrypt: El flujo de memoria donde se escribirán los datos cifrados.
                //@encryptor: El transformador criptográfico que realiza el cifrado.
                //CryptoStreamMode.Write: El modo de escritura, que indica que se escribirán datos cifrados en el flujo.
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

        //Método desenpriptar texto de Memoria a Texto
        //@cipherText = Texto cifrado
        //@Aesalg = Objeto de la clase Aes, que contiene la clave y el vector de inicialización.
        static string DesencriptarMemoriaATexto(byte[] cipherText, Aes aesAlg)
        {
            string textoplano = null;
            //Creamos el objeto de descifrar AES
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            //En vez de un fichero vamos a hacer uso de la memoria para guardar los datos. Haciendo uso de la clase MemoryStream.
            //En vez de un try-catch , se hace un using para que al finalizar el bloque de código se cierre el stream y no haya fugas de memoria.
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                //Crea el flujo de descifrado:se encarga de desencriptar un texto cifrado en memoria y convertirlo de nuevo a texto plano. 
                //@msDecrypt: Es un objeto MemoryStream que contiene los datos cifrados en memoria. Este flujo de memoria se utiliza como origen de datos para la desencriptación.
                //@decryptor:  Es un objeto ICryptoTransform que se crea utilizando el algoritmo de desencriptación AES y la clave y el vector de inicialización proporcionados. Este objeto se utiliza para realizar la desencriptación.
                //CryptoStreamMode.Read:  Es el modo de lectura del CryptoStream, que indica que se leerán datos desencriptados desde el flujo.
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
