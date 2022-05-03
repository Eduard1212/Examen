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
        Client _client = null; // клиент из БД которого мы передали на главной форме, нажав на кнопку Редактировать
        #endregion

        public ClientsAddEdit(Client client)
        {
            InitializeComponent();
            if (client != null) // проверям не пуст ли клиент, иначе создаем нового, происходит когда тыкаешь Добавить клиента на главной форме
            {
                _client = client;
            }
            else _client = new Client();
        }       // инициализатор класса

        private void ClientsAddEdit_Load(object sender, EventArgs e)
        { // загрузка формы добавления и редактирования клиентов, задаем во все поля инфу о клиенте, если она есть
            #region Загрузка полей формы
            this.phone.Text = _client.Phone;
            this.email.Text = _client.Email;
            this.photo.Text = _client.PhotoPath;
            this.id.Text = _client.ID.ToString();
            this.surname.Text = _client.FirstName;
            this.name.Text = _client.LastName;
            this.patro.Text = _client.Patronymic;
            this.calendar.SetDate(Convert.ToDateTime(_client.Birthday));

            if (File.Exists(_client.PhotoPath)) // проверяем существует ли путь для фотки, указанный в базе, если да то вешаем картинку на пикчербокс
            {
                pictureBox1.Image = Image.FromFile(_client.PhotoPath);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
            #endregion
        } // загрузка формы

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {//ддобавляем нового клиента или редактируем того что нам сюда передали с прошлой формы
                //_client.ID = Convert.ToInt32(id.Text);  //-- закомментировал потому, что БД не дает менять ID из вне
                _client.FirstName = surname.Text;
                _client.LastName = name.Text;
                _client.PhotoPath = photo.Text;
                _client.Patronymic = patro.Text;
                _client.Email = email.Text;
                _client.Phone = phone.Text;
                _client.Birthday = (DateTime?)calendar.SelectionStart;
                if (id.Text == "0")
                {
                    _client.RegistrationDate = DateTime.Now;
                    Clients.GetClients().Client.Add(_client);
                }
                Clients.GetClients().SaveChanges();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Ошибка", MessageBoxButtons.OK);
                return;
            }
            DialogResult result = MessageBox.Show("Клиент успешно обновлен!","Успех",MessageBoxButtons.OKCancel);
            if(result == DialogResult.OK) // если в появившемся окне клиент выбирает окей то закрываем данную формы, иначе даем еще отредактировать клиента
                this.Close();
        } // обработка кнопки сохранить

        private void button1_Click(object sender, EventArgs e)
        { // функция обрабатывает работу с картинкой, нажимаем обзор и выбираем с проводника фотку, дальше код натягивает ее на пикчербокс
          // и сохраняет путь к ней в поле под картинкой, откуда мы и возьмем этот путь при сохранении
            OpenFileDialog file = new OpenFileDialog();
            using (file)
            {
                file.InitialDirectory = @"D:\\Edward\Ucheba\УП02-4курс\УП02-4курс\МутовинЭ.Е";
                file.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
                file.FilterIndex = 2;
                file.RestoreDirectory = true;
                if (file.ShowDialog() == DialogResult.OK)
                {
                    this.photo.Text = file.FileName;
                    pictureBox1.Image = Image.FromFile(this.photo.Text);
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
        } // обработка выбора фотки
    }
}
