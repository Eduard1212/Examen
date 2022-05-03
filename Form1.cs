using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
/// <summary>
/// Дизайн вообще нужно делать в стиле который указан в задании
/// Если же там ничего не указано то делай как тебе захочется
/// 
/// К тому же, не обязательно создавать для редактирования и создания новую форму как я,
/// можно просто добавить поля прям на эту форму и выводить инфу в них, при клике по таблице
/// 
/// Да и с тем чтобы в полях имя, фамилия, отчество высвечивалась подсказка лучше не парься, я сделал просто чтобы показать что это возможно
/// 
/// </summary>
namespace Examen
{
    public partial class Form1 : Form
    {
        //переменные формы
        private int start = 0; // поле показывающее какая страничка в табличке сейчас активна, тобишь если мы на 3 странице, тогда переменная равна 3
        private double end; // переменная показывает сколько всего страниц у нас получилось вывести в таблицу
        public int _ID; // код клиента, нужен для того чтобы найти того самого клиента и передать его на форму редактирования
        public Form1()
        {
            InitializeComponent();
        }

        // загрузка формы
        private void Form1_Load(object sender, EventArgs e)
        {
            clientsGrid.Rows.Clear();
            try
            {    
                clientsGrid.AutoGenerateColumns = false;
                clientsGrid.ColumnCount = 0;
                clientsGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ID", HeaderText="Идентификатор"});
                clientsGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FirstName", HeaderText = "Фамилия" });
                clientsGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "LastName", HeaderText = "Имя" });
                clientsGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Patronymic", HeaderText = "Отчество" });
                clientsGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Birthday", HeaderText = "Дата рождения" });
                clientsGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Phone", HeaderText = "Телефон" });
                clientsGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Email", HeaderText = "Почта" });
                clientsGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "RegistrationDate", HeaderText = "Дата регистрации" });
                clientsGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Tag", HeaderText = "Тэги" });
                clientsGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PhotoPath", HeaderText = "Тэги", Visible=false });
                GetDataSource("Load", "");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            this.count.Text = String.Format("{0}  из {1}", start + 1, (int)end);

            textBox1_MouseLeave(textBox1, null);
            textBox2_MouseLeave(textBox2, null);
            textBox3_MouseLeave(textBox3, null);
            comboBox1.Items.Clear();
            comboBox1.Items.Insert(0, "");
            comboBox1.Items.Insert(1, "А - Я(Фамилия)");
            comboBox1.Items.Insert(2, "Я - А(Фамилия)");
            comboBox1.Items.Insert(3, "По дате регистрации");
            comboBox1.Items.Insert(4, "По дате рождения");
            comboBox1.SelectedIndex = 0;
            
        }
        //получение данных из БД
        private void GetDataSource(string mode, string value)
        {
            // здесь мы делаем сортировку входящих данных, т.е. "mode" и "value"
            // mode в моем случае нужен для того чтобы определить что мы делаем, просто выводим данные или сортируем/фильтруем таблицу
            // Value же нужен для того чтобы понимать как сортировать таблицу, зависит value от того что ты выберешь в комбобоксе
            switch (mode)
            {
                case "Load": // обычная загрузка без сортировки
                    try
                    {
                        clientsGrid.DataSource = Clients.GetClients().Client.OrderBy(Client => Client.ID).Skip(start * 10).Take(10).ToList();
                        clientsGrid.ClearSelection();
                        end = Math.Ceiling((double)Clients.GetClients().Client.Count() / 10.00);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error");
                    }
                    break;
                case "Name": // также загрузка но только тех людей у кого имя содержит те буквы что ты пишешь в соответствующее поле 
                    try
                    {
                        clientsGrid.DataSource = Clients.GetClients().Client.Where(Client => Client.LastName.Contains(value)).ToList();
                        clientsGrid.ClearSelection();
                        end = Math.Ceiling((double)Clients.GetClients().Client.Count() / 10.00);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error");
                    }
                    break;
                case "SName": // тоже что выше только с фамилией
                    try
                    {
                        clientsGrid.DataSource = Clients.GetClients().Client.Where(Client => Client.FirstName.Contains(value)).ToList();
                        clientsGrid.ClearSelection();
                        end = Math.Ceiling((double)Clients.GetClients().Client.Count() / 10.00);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error");
                    }
                    break;
                case "Patro": // тоже самое, только с отчеством
                    try
                    {
                        clientsGrid.DataSource = Clients.GetClients().Client.Where(Client => Client.Patronymic.Contains(value)).ToList();
                        clientsGrid.ClearSelection();
                        end = Math.Ceiling((double)Clients.GetClients().Client.Count() / 10.00);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error");
                    }
                    break;
                case "Sort": 
                    // здесь уже сортировка идет, зависящая от комбобокса, так как в комбобоксе несколько значений
                    // то и вариантов сортировки тоже несколько, изза чего мы и разбиваем данный пункт
                    // на несколько подпунктов
                    switch (value)
                    {
                        case "A-Z": // первый пункт По алфвиту
                            try
                            {
                                clientsGrid.DataSource = Clients.GetClients().Client.OrderBy(Client => Client.FirstName).Skip(start * 10).Take(10).ToList();
                                clientsGrid.ClearSelection();
                                end = Math.Ceiling((double)Clients.GetClients().Client.Count() / 10.00);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Error");
                            }
                            break;
                        case "Z-A": // второй пункт, против алфавита, или в обратном порядке алфавита, тут как удобнее
                            try
                            {
                                clientsGrid.DataSource = Clients.GetClients().Client.OrderByDescending(Client => Client.FirstName).Skip(start * 10).Take(10).ToList();
                                clientsGrid.ClearSelection();
                                end = Math.Ceiling((double)Clients.GetClients().Client.Count() / 10.00);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Error");
                            }
                            break;
                        case "Reg": // сортировка по дате регистрации, по возрастанию
                            try
                            {
                                clientsGrid.DataSource = Clients.GetClients().Client.OrderBy(Client => Client.RegistrationDate).Skip(start * 10).Take(10).ToList();
                                clientsGrid.ClearSelection();
                                end = Math.Ceiling((double)Clients.GetClients().Client.Count() / 10.00);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Error");
                            }
                            break;
                        case "Birth": // сортировка по дате рождения, по возрастанию
                            try
                            {
                                clientsGrid.DataSource = Clients.GetClients().Client.OrderBy(Client => Client.Birthday).Skip(start * 10).Take(10).ToList();
                                clientsGrid.ClearSelection();
                                end = Math.Ceiling((double)Clients.GetClients().Client.Count() / 10.00);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Error");
                            }
                            break;
                        default: // значение дефолт, происходит если пихнешь что то не то, просто ничего не произойдет и не сломается
                            break;
                    }
                    break;
                default:
                    break;
            }
            
        }
        
        //обработка кнопки редактировать
        private void editBtn_Click(object sender, EventArgs e)
        {
            if (this.clientsGrid.SelectedRows.Count != 0)
            {
                _ID = Convert.ToInt32(this.clientsGrid.SelectedRows[0].Cells[0].Value);
                Client client = Clients.GetClients().Client.Where(c => c.ID == _ID).FirstOrDefault() as Client;
                ClientsAddEdit form = new ClientsAddEdit(client);
                form.Show();
                GetDataSource("Load", "");
            }
            else MessageBox.Show("Для редактирования необходимо выделить запись, кликнув по необходимой строке.","Внимание!!!",MessageBoxButtons.OK);
        }
        //обработка кнопки удалить
        private void deleteBtn_Click(object sender, EventArgs e)
        {
            if (this.clientsGrid.SelectedRows.Count != 0)
            {
                DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить данного клиента из базы данных?\nВосстановление будет невозможно!", "Внимание!!!", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    _ID = Convert.ToInt32(this.clientsGrid.SelectedRows[0].Cells[0].Value);
                    try
                    {
                        Clients.GetClients().Client.Remove(Clients.GetClients().Client.Where(c => c.ID == _ID).FirstOrDefault() as Client);
                        Clients.GetClients().SaveChanges();
                    }
                    catch (Exception ex)
                    { MessageBox.Show(ex.Message); }
                }
            }
            else MessageBox.Show("Для удаления необходимо выделить запись, кликнув по необходимой строке.", "Внимание!!!", MessageBoxButtons.OK);
            GetDataSource("Load", "");
        }

        // обработка кнокпки <
        private void button1_Click(object sender, EventArgs e)
        {
            if (start >= 1)
                start--;
            this.count.Text = String.Format("{0}  из {1}", start + 1, (int)end);
            GetDataSource("Load", "");
        }
        // обработка кнокпки >
        private void button2_Click(object sender, EventArgs e)
        {
            if (start < end - 1)
                start++;
            this.count.Text = String.Format("{0}  из {1}", start + 1, (int)end);
            GetDataSource("Load", "");
        }
        // обработка кнокпки Добавить клиента
        private void button3_Click(object sender, EventArgs e)
        {
            ClientsAddEdit form = new ClientsAddEdit(null);
            form.Show();
        }

       
        /// <summary>
        /// Все что дальше написано нужно для того чтобы обработать фильтрацию в реальном времени
        /// То есть ты меняешь текст и таблица СРАЗУ ЖЕ сортируется
        /// </summary>

        //обработка события происходящего когда мышка ВЫХОДИТ за рамки поля Фамилия
        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Фамилия";
                textBox1.ForeColor = Color.Gray;
            }
        }
        //обработка события происходящего когда мышка ВХОДИТ в зону поля Фамилия
        private void textBox1_MouseEnter(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox1.Text == "Фамилия")
            {
                textBox1.ForeColor = Color.Empty;
                textBox1.Clear();
            }
        }

        //обработка события происходящего когда мышка ВЫХОДИТ за рамки поля Имя
        private void textBox2_MouseLeave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "Имя";
                textBox2.ForeColor = Color.Gray;
            }
        }
        //обработка события происходящего когда мышка ВХОДИТ в зону поля Имя
        private void textBox2_MouseEnter(object sender, EventArgs e)
        {
            if (textBox2.Text == "" || textBox2.Text == "Имя")
            {
                textBox2.ForeColor = Color.Empty;
                textBox2.Clear();
            }
        }

        //обработка события происходящего когда мышка ВЫХОДИТ за рамки поля Отчество
        private void textBox3_MouseLeave(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                textBox3.Text = "Отчество";
                textBox3.ForeColor = Color.Gray;
            }
        }
        //обработка события происходящего когда мышка ВХОДИТ в зону поля Имя, то есть когда наводишь на текстбокс
        private void textBox3_MouseEnter(object sender, EventArgs e)
        {
            if (textBox3.Text == "" || textBox3.Text == "Отчество")
            {
                textBox3.ForeColor = Color.Empty;
                textBox3.Clear();
            }
        }

        // событие происходящее когда ты меняешь фамилию
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text != "Фамилия" && textBox1.Text != "")
                GetDataSource("SName", textBox1.Text);
        }
        // событие происходящее когда ты меняешь имя
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text != "Имя" && textBox2.Text != "")
                GetDataSource("Name", textBox2.Text);
        }
        // событие происходящее когда ты меняешь отчество
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text != "Отчество" && textBox3.Text != "")
                GetDataSource("Patro", textBox3.Text);
        }

        /// <summary>
        /// Данный код нужен для обработки значение в комбобоксе
        /// меняешь значение - табличка сортируется 
        /// </summary>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            switch (box.SelectedIndex)
            {
                case 0:
                    GetDataSource("Load","");
                    break;
                case 1:
                    GetDataSource("Sort","A-Z");
                    break;
                case 2:
                    GetDataSource("Sort", "Z-A");
                    break;
                case 3:
                    GetDataSource("Sort", "Reg");
                    break;
                case 4:
                    GetDataSource("Sort", "Birth");
                    break;
                default:
                    break;
            }
        }
    }
}
