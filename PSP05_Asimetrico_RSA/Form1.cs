using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace PSP05_Asimetrico_RSA
{
    public partial class Form1 : Form
    {
        private byte[] bytextoCifrado;
        private string publicKeyFile = "publicKey.xml";
        private string privateKeyFile = "privateKey.xml";

        public Form1()
        {

            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Generar claves
            generarClaves(publicKeyFile, privateKeyFile);
            bytextoCifrado = encriptar(publicKeyFile, Encoding.UTF8.GetBytes(this.textBox1.Text));
            this.label4.Text = Encoding.UTF8.GetString(bytextoCifrado).ToLower().Replace("-","");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] desencriptado = Desencriptar(privateKeyFile, bytextoCifrado);
            this.label2.Text = Encoding.UTF8.GetString(desencriptado);
        }


        //Método que genera tanto clave pública como clave privada para hacer uso de algoritmo asimétrico.
        //@string publicKF: Nombre del fichero donde se guarda la clave pública
        //@string privateKF: Nombre del fichero donde se guarda la clave privada
        private static void generarClaves(string publicKF, string privateKF)
        {
            //Creamos un objeto de tipo RSACryptoServiceProvider para hacer uso de la clave pública y clave privada para su poserior uso.
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                //Obtiene o establece un valor que indica si la clave debe conservarse en el proveedor de servicios criptográficos (CSP).
                //Le indicamos el valor a false porque no queremos que esté en ningún proveedor de servicios.
                rsa.PersistKeyInCsp = false;

                //Borramos cualquier fichero que contenga los mismos nombres
                if (File.Exists(publicKF))
                        File.Delete(publicKF);
                if(File.Exists(privateKF))
                        File.Delete(privateKF);


                //ToXmlString(false): Crea un string con la clave pública. Para que sea pública hay que pasarle como parámetro (false).
                string publicKey = rsa.ToXmlString(false);

                //Crea un fichero y guarda el texto de la clave en el fichero.
                File.WriteAllText(publicKF, publicKey);


                //Mismo proceso anterior para la clave privada
                string privateKey = rsa.ToXmlString(true);
                File.WriteAllText(privateKF, privateKey);

            }
        }

        public static byte[] encriptar(string publicKF, byte[] textoPlano)
        {
            byte[] encriptado;

            //Se crea un objeto de tipo RSACryptoServiceProvider para poder hacer uso de sus métodos de encriptación
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                //Le indicamos el valor a false porque no queremos que esté en ningún proveedor de servicios.
                rsa.PersistKeyInCsp = false;

                //Lee el contenido del fichero y lo guarda en un string
                string publicKey = File.ReadAllText(publicKF);

                //FromXmlString(publicKey): Inicializa un objeto RSA de la información de clave de una cadena XML.
                rsa.FromXmlString(publicKey);

                //Cifra los datos con el algoritmo RSA.
                //@textoPlano: datos que se van a cifrar
                //@Booleano: true para realizar el cifrado RSA directo mediante el relleno de OAEP (solo disponible en equipos con Windows XP o versiones posteriores como en nuestro caso); de lo contrario, false para usar el relleno PKCS#1 v1.5.
                encriptado = rsa.Encrypt(textoPlano, true);

            }

            //Valor que se devuelve
            return encriptado;
            
        }



        public static byte[] Desencriptar(string privateKF, byte[] textoEncriptado)
        {
            
            byte[] desencriptado;
            //Se crea un objeto de tipo RSACryptoServiceProvider para poder hacer uso de sus métodos.
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                //Le indicamos el valor a false porque no queremos que esté en ningún proveedor de servicios.
                rsa.PersistKeyInCsp = false;


                //Lee el contenido del fichero y lo guarda en un string
                string privateKey = File.ReadAllText(privateKF);

                //FromXmlString(false): Inicializa un objeto RSA de la información de clave de una cadena XML.
                //En este caso clave privada ya que la utilizaremos para descifrar
                rsa.FromXmlString(privateKey);

                //Descifra los datos que se cifraron anteriormente.
                //@textoEncriptado: Datos que se van a descifrar.
                //@Booleano: true para realizar el cifrado RSA directo mediante el relleno de OAEP (solo disponible en equipos con Windows XP o versiones posteriores como en nuestro caso); de lo contrario, false 
                desencriptado = rsa.Decrypt(textoEncriptado, true);

            }
            return (desencriptado);

        }
   
    }
}

     