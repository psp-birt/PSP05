//Importar librerías requeridas
using System.Security.Cryptography;

namespace PSP05_Hash_form
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            String nombreFichero = String.Empty;
            //Abrimos el dialogo para elegir el fichero
            
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Recogemos el nombre del fichero
                nombreFichero = openFileDialog1.FileName;
                //Abrimos fichero y creamos un objeto stream para tratarlo.
                FileStream stream =File.OpenRead(nombreFichero);

                //Creamos objeto MD5 para creación de hash con método correspondiente
                MD5 mD5 = MD5.Create();

                // Como parámetro se le pasa un stream y devuelve los cálculos del resumen en un array de 16 bytes
                byte[] hash = mD5.ComputeHash(stream);

                //Este array de 16bytes, se puede representar y mostrar como 32 digitos en hexadecimales (habitual)
                this.label1.Text += BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }




        }

        private void button2_Click(object sender, EventArgs e)
        {
            String nombreFichero = String.Empty;
            //Abrimos el dialogo para elegir el fichero

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Recogemos el nombre del fichero
                nombreFichero = openFileDialog1.FileName;
                //Abrimos fichero y creamos un objeto stream para tratarlo.
                FileStream stream = File.OpenRead(nombreFichero);

                //Creamos objeto SHA256 para creación de hash con método correspondiente
                SHA256 sha256Hash = SHA256.Create();

                // Como parámetro se le pasa un stream y devuelve los cálculos del resumen en un array de 16 bytes
                byte[] hash = sha256Hash.ComputeHash(stream);

                //Este array de 16bytes, se puede representar y mostrar como 32 digitos en hexadecimales (habitual)
                this.label5.Text = "HEXA:" + BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            String nombreFichero = String.Empty;
            String valorHashFichero = String.Empty;
            String valorHashFicheroMostrar = String.Empty;
            //Abrimos el dialogo para elegir el fichero

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Recogemos el nombre del fichero
                nombreFichero = openFileDialog1.FileName;
                //Abrimos fichero y creamos un objeto stream para tratarlo.
                FileStream stream = File.OpenRead(nombreFichero);

                //Creamos objeto SHA256 para creación de hash con método correspondiente
                SHA256 sha256Hash = SHA256.Create();

                // Como parámetro se le pasa un stream y devuelve los cálculos del resumen en un array de 16 bytes
                byte[] hash = sha256Hash.ComputeHash(stream);

                //Este array de 16bytes, se puede representar y mostrar como 32 digitos en hexadecimales (habitual)
                //this.label5.Text = "HEXA:" + BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                valorHashFichero = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                valorHashFicheroMostrar = "HEXA:" + valorHashFichero;
            }

            if (textBox1.Text.Equals(valorHashFichero))
            {
                this.label3.Text = "El hash coincide.";
            }
            else
            {
                this.label3.Text = "El hash NO coincide.";
            }

        }
    }
}

//Valor HASH SHA:3ccf8ed68d4b63273e7a8c6334cfca92db0d1a9e778c332a7ba347fc45729750