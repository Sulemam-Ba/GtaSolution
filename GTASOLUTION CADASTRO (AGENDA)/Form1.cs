using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//FORAM USADO ESTA 2 BIBLIOTECA PARA ESTA APLIACAÇÃO
using System.Data.SqlClient;
using System.IO;

namespace GTASOLUTION_CADASTRO__AGENDA_
{
    public partial class AGENDA : Form
    {
        //DECLARAÇÃO DAS VARIÁVEIS
        SqlConnection conexão;
        SqlCommand comando;
        SqlDataAdapter da;
        SqlDataReader dr;
        Bitmap bmp;

        string strSQL;

        public AGENDA()
        {
            InitializeComponent();
        }

        //BOTÃO ADICIONAR
        private void ADICIONAR_Click(object sender, EventArgs e)
        {
            try
            {// STRING DE INTEGRAÇÃO COM BANCO DE DADOS
                conexão = new SqlConnection("Data Source=DESKTOP-51ADQ9D\\SQLEXPRESS;Initial Catalog=GTASOLUTION;Integrated Security=True");

                //STRING DE INSERÇÃO A TABELA 
                strSQL = "INSERT INTO gtatabela ( nome, telefone, endereço, cpf, rg, foto) VALUES ( @NOME, @TELEFONE, @ENDEREÇO, @CPF, @RG, @FOTO )";

                comando = new SqlCommand(strSQL, conexão);

                //CODIGO DE CONVERSÃO DE IMAGEM PARA O BANCO DE DADOS
                Image img = pictureBox2.Image;
                byte[] arr;
                ImageConverter converter = new ImageConverter();
                arr = (byte[])converter.ConvertTo(img, typeof(byte[]));

                //CODIGO PARA ADICIONAR OS VALORES DOS TEXTBOX NA TABELA 
                comando.Parameters.AddWithValue("@NOME", textBox2.Text);
                comando.Parameters.AddWithValue("@TELEFONE", textBox3.Text);
                comando.Parameters.AddWithValue("@ENDEREÇO", textBox4.Text);
                comando.Parameters.AddWithValue("@CPF", textBox5.Text);
                comando.Parameters.AddWithValue("@RG", textBox6.Text);
                comando.Parameters.AddWithValue("@FOTO", arr);

                //LIMPAR OS CAMPOS
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();

                //LIMPA O CAMPO DAS FOTOS 
                pictureBox2.Image = null;

                //ABRE CONEXÃO DO BANCO E RETORNA MENSAGEM DE CASTRAMENTO COM SUCESSO
                conexão.Open();
                comando.ExecuteNonQuery();
                MessageBox.Show("Usuário cadastrado com seucesso");

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                //FECHANDO CONEXÃO COM BANCO DE DADOS
                conexão.Close();
                conexão = null;
                comando = null;
            }
        }

        //BOTÃO EXIBIR
        private void EXIBIR_Click(object sender, EventArgs e)
        {
            try
            {
                // INTEGRAÇÃO COM BANCO DE DADOS
                conexão = new SqlConnection("Data Source=DESKTOP-51ADQ9D\\SQLEXPRESS;Initial Catalog=GTASOLUTION;Integrated Security=True");

                //EXIBIR BANCO DE DADOS 
                strSQL = "SELECT * FROM  gtatabela";

                DataSet ds = new DataSet();

                da = new SqlDataAdapter(strSQL, conexão);

                conexão.Open();

                da.Fill(ds);

                dataGridView1.DataSource = ds.Tables[0];

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexão.Close();
                conexão = null;
                comando = null;
            }
        }

        private void CONSULTAR_Click(object sender, EventArgs e)
        {
            try
            {
                // INTEGRAÇÃO COM BANCO DE DADOS
                conexão = new SqlConnection("Data Source=DESKTOP-51ADQ9D\\SQLEXPRESS;Initial Catalog=GTASOLUTION;Integrated Security=True");

                //SELECIONAR TABELA POR ID
                strSQL = "SELECT * FROM gtatabela WHERE ID = @ID";

                comando = new SqlCommand(strSQL, conexão);

                comando.Parameters.AddWithValue("@ID", textBox1.Text);


                conexão.Open();
                dr = comando.ExecuteReader();

                while (dr.Read())
                {
                    textBox2.Text = (string)dr["nome"];
                    textBox3.Text = (string)dr["telefone"];
                    textBox4.Text = (string)dr["endereço"];
                    textBox5.Text = (string)dr["cpf"];
                    textBox6.Text = (string)dr["rg"];

                    textBox2.Text = Convert.ToString(dr["nome"]);
                    textBox3.Text = Convert.ToString(dr["telefone"]);
                    textBox4.Text = Convert.ToString(dr["endereço"]);
                    textBox5.Text = Convert.ToString(dr["cpf"]);
                    textBox6.Text = Convert.ToString(dr["rg"]);
                    MemoryStream ms = new MemoryStream((byte[])dr["foto"]);
                    pictureBox2.Image = Image.FromStream(ms);

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexão.Close();
                conexão = null;
                comando = null;
            }
        }

        private void EDITAR_Click(object sender, EventArgs e)
        {
            try
            {
                // INTEGRAÇÃO COM BANCO DE DADOS
                conexão = new SqlConnection("Data Source=DESKTOP-51ADQ9D\\SQLEXPRESS;Initial Catalog=GTASOLUTION;Integrated Security=True");
                
                //STRING DE ALTERAÇÃO DO TABELA 
                strSQL = "UPDATE gtatabela SET NOME = @NOME, TELEFONE = @TELEFONE, ENDEREÇO = @ENDEREÇO, CPF = @CPF, RG = @RG WHERE ID = @ID";


                comando = new SqlCommand(strSQL, conexão);

                //CODIGO PARA ADICIONAR OS VALORES DOS TEXTBOX NA TABELA 
                comando.Parameters.AddWithValue("@ID", textBox1.Text);
                comando.Parameters.AddWithValue("@NOME", textBox2.Text);
                comando.Parameters.AddWithValue("@TELEFONE", textBox3.Text);
                comando.Parameters.AddWithValue("@ENDEREÇO", textBox4.Text);
                comando.Parameters.AddWithValue("@CPF", textBox5.Text);
                comando.Parameters.AddWithValue("@RG", textBox6.Text);

                //LIMPAR TODOS CAMPOS
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();

                pictureBox2.Image = null;

                conexão.Open();
                comando.ExecuteNonQuery();
                MessageBox.Show("Usuário editado com seucesso");

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexão.Close();
                conexão = null;
                comando = null;
            }
        }

        private void EXCLUIR_Click(object sender, EventArgs e)
        {
            try
            {// STRING DE INTEGRAÇÃO COM BANCO DE DADOS
                conexão = new SqlConnection("Data Source=DESKTOP-51ADQ9D\\SQLEXPRESS;Initial Catalog=GTASOLUTION;Integrated Security=True");

                //STRING DE EXCLUSÃO DA TABELA POR ID
                strSQL = "DELETE gtatabela WHERE ID = @ID";

                comando = new SqlCommand(strSQL, conexão);

                comando.Parameters.AddWithValue("@ID", textBox1.Text);

                //LIMPAR TODOS CAMPOS
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                pictureBox2.Image = null;


                conexão.Open();
                comando.ExecuteNonQuery();
                MessageBox.Show("Usuário deletado com seucesso");

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexão.Close();
                conexão = null;
                comando = null;
            }
        }
        //LOCALIZAR IMAGEM PARA INSERIR NO BANCO DE DADOS
        private void BUSCARFOTO_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.ImageLocation = openFileDialog1.FileName;
                string nome = openFileDialog1.FileName;

                bmp = new Bitmap(nome);

                pictureBox2.Image = bmp;

            }
        }

        private void LIMPAR_Click(object sender, EventArgs e)
        {
            //LIMPAR TODOS CAMPOS
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            pictureBox2.Image = null;
        }
    }
}
