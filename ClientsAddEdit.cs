using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Examen
{
    public partial class ClientsAddEdit : Form
    {
        #region Переменные формы
        private int _id;
        private string _surname;
        private string _name;
        private string _patro;
        private DateTime _birth;
        private string _mail;
        private string _phone;
        private string _photo;
        Client _client = null;
        #endregion

        public ClientsAddEdit(Client client)
        {
            InitializeComponent();
            this._id = client.ID;
            this._surname = client.FirstName;
            this._name = client.LastName;
            this._patro = client.Patronymic;
            this._birth = Convert.ToDateTime(client.Birthday);
            this._mail = client.Email;
            this._phone = client.Phone;
            this._photo = client.PhotoPath;
            _client = client;
        }       // инициализатор класса

        private void ClientsAddEdit_Load(object sender, EventArgs e)
        {
            #region Загрузка полей формы
            this.phone.Text = _phone;
            this.email.Text = _mail;
            this.photo.Text = _photo;
            this.id.Text = _id.ToString();
            this.surname.Text = _surname;
            this.name.Text = _name;
            this.patro.Text = _patro;
            this.calendar.SetDate(_birth);

            if (File.Exists(_photo))
            {
                pictureBox1.Image = Image.FromFile(this._photo);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
            #endregion
        } // загрузка формы

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //_client.ID = Convert.ToInt32(id.Text);  -- закомментировал потому, что БД не дает менять ID из вне
                _client.FirstName = surname.Text;
                _client.LastName = name.Text;
                _client.PhotoPath = _photo;
                _client.Patronymic = patro.Text;
                _client.Email = email.Text;
                _client.Phone = phone.Text;
                _client.Birthday = calendar.SelectionStart;
                Clients.GetClients().SaveChanges();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Ошибка", MessageBoxButtons.OK);
                return;
            }
            DialogResult result = MessageBox.Show("Клиент успешно обновлен!","Успех",MessageBoxButtons.OKCancel);
            if(result == DialogResult.OK)
                this.Close();
        } // обработка кнопки сохранить

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            using (file)
            {
                file.InitialDirectory = @"D:\\Edward\Ucheba\УП02-4курс\УП02-4курс\МутовинЭ.Е";
                file.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
                file.FilterIndex = 2;
                file.RestoreDirectory = true;
                if (file.ShowDialog() == DialogResult.OK)
                {
                    this.photo.Text = this._photo = file.FileName;
                    pictureBox1.Image = Image.FromFile(this._photo);
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
        } // обработка выбора фотки
    }
}
