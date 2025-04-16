using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSP05_Simetrico_Sustitucion
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String alfabeto = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZ0123456789 ";
            String alfabetoCifrado = "KLMNÑOPQRSTUVWXYZ0123456789 ABCDEFGHIJ"; //Desplazamiendo del alfabeto en 10 posiciones

            String mensaje, mensajeCifrado = "";
            mensaje = this.textBox1.Text;
            for (int i = 0; i < mensaje.Length; i++)
            {
                mensajeCifrado += alfabetoCifrado.Substring(alfabeto.IndexOf(mensaje.Substring(i, 1).ToUpper()), 1);
            }
            this.label1.Text = mensajeCifrado;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String alfabeto = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZ0123456789 ";
            String alfabetoCifrado = "KLMNÑOPQRSTUVWXYZ0123456789 ABCDEFGHIJ"; //Desplazamiendo del alfabeto en 10 posiciones

            String mensaje, mensajeOriginal = "";
            mensaje = this.label1.Text;
            for (int i = 0; i < mensaje.Length; i++)
            {
                mensajeOriginal += alfabeto.Substring(alfabetoCifrado.IndexOf(mensaje.Substring(i, 1).ToUpper()), 1);
            }
            this.label2.Text += mensajeOriginal;
        }


    }
}
