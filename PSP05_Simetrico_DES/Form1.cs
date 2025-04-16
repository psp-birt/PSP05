using System;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.IO;

namespace PSP05_Simetrico_DES
{
    public partial class Form1 : Form
    {
        private DES objDES = null;
        private string fichero = String.Empty;

        public Form1()
        {
            InitializeComponent();

            //Creamos el objeto objDES para conseguir la clave única para cifrar y descifrar
            //al crear un objeto DES se genera la key y el vector IV
            //Se crea un objeto criptográfico objDES
            
            objDES = DES.Create();

            //Creanis un fichero donde guardaremos el mensaje cifrado.
            fichero = "encriptaDES.txt";
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Try-catch ya que trabajaremos con ficheros en método encriptar
            try
            {
                //Variables requeridas
                string textoCifrado = string.Empty;

                //Llamada a método encriptar
                textoCifrado = EncriptarTextoAFichero(this.textBox1.Text, fichero, objDES.Key, objDES.IV);
                this.label4.Text = textoCifrado;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
           


            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Try-catch ya que trabajaremos con ficheros en método desencriptar
            try
            {
                //Variables requeridas

                //Llamada a método desencriptar
                string descifrado = DesencriptarFicheroATexto(fichero, objDES.Key, objDES.IV);
                this.label2.Text = descifrado;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }



        }

        //Método encriptar texto a Fichero
        //@data = Texto a cifrar
        //@FileName = Nombre de fichero donde guardaremos el texto cifrado
        //@Key = Clave generada
        //@IV = Vector generado
        public static string EncriptarTextoAFichero(String Data, String FileName, byte[] Key, byte[] IV)
        {
            try 
            {
                //Abrimos el fichero para poder escribir sobre el mismo.
                FileStream fstream = File.Open(FileName, FileMode.Create);

                //Creamos un objeto de tipo DES para asignarle el key y el vector que utilizaremos
                DES DESalog = DES.Create();
                DESalog.Key = Key;
                DESalog.IV = IV;
                var ojb = DESalog.CreateEncryptor(Key, IV);


                //Crea un objeto de CryptoStream. Este objeto sirve para unir el flujo común de datos con transformaciones criptográficas.
                //Pasamos como parámetro.
                //@fStream = El flujo en el que se vuelca la transformación criptográfica.
                //@DESalg.CreateEncryptor(Key, IV)  = Modalidad de transformación criptográfica. Algoritmo DES, con clave y vector.
                //@CryptoStreamMode.Write = Modo en el que se realizará la criptografía- En este caso escritura.
                CryptoStream cStream = new CryptoStream(fstream, ojb, CryptoStreamMode.Write);

                //A partir de aquí creamos un StreamWriter y se trata con los métodos habituales de Stream

                StreamWriter sWriter = new StreamWriter(cStream);
                sWriter.Write(Data);
                //Cierre de flujos
                sWriter.Close();
                cStream.Close();

                //Volvemos a abrir el fichero para lee el contenido y mostrarlo en el label
                fstream = File.Open(FileName, FileMode.Open);
                StreamReader sr = new StreamReader(fstream);
                string cifrado = sr.ReadToEnd();

                sr.Close();
                fstream.Close();

                //Devuelve el texto cifrado
                return cifrado;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Error criptofráfico: {0}", e.Message);
                return null;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Error de acceso a fichero: {0}", ex.Message);
                return null;
            }

        }
        public static string DesencriptarFicheroATexto(String FileName, byte[] Key, byte[] IV)
        {
            try
            {
                //Abrimos fichero para leer contenido cifrado
                FileStream fstream = File.Open(FileName, FileMode.Open);
                // Creamos objeto DES para posteriormente pasarlo como parámetro en CryptoStream
                DES DESalog = DES.Create();
                var cryto = DESalog.CreateDecryptor(Key, IV);



                //Crea un objeto de CryptoStream. Este objeto sirve para unir el flujo común de datos con transformaciones criptográficas. Desciframos el contenido del fichero.
                //Pasamos como parámetro.
                //@fStream =Flujo desde el cual vamos a obtener los datos para descifrar.
                //@DESalg.CreateEncryptor(Key, IV)  = Modalidad de transformación criptográfica. Algoritmo DES, con clave y vector.
                //@CryptoStreamMode.Read = Modo en el que se realizará la criptografía- En este caso lectura

                CryptoStream cStream = new CryptoStream(fstream, cryto, CryptoStreamMode.Read);

                //Recogemos en un string el texto descifrado
                StreamReader sr = new StreamReader(cStream);
                string descifrado = sr.ReadToEnd();

                //Cerramos los flujos

                sr.Close();
                cStream.Close();
                fstream.Close();


                //Se devuelve el texto descifrado.
                return descifrado;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Error criptofráfico: {0}", e.Message);
                return null;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Error de acceso a fichero: {0}", ex.Message);
                return null;
            }



        }
        
    }
}


