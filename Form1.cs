﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Examen
{
    public partial class Form1 : Form
    {
        private int start = 0;
        private double end;
        public int _ID;
        public Form1()
        {
            InitializeComponent();
        }

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
        }
        private void GetDataSource(string mode, string value)
        {
            
            switch (mode)
            {
                case "Load":
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
                case "Name":
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
                case "SName":
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
                case "Patro":
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
                default:
                    break;
            }
            
        }
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (start >= 1)
                start--;
            this.count.Text = String.Format("{0}  из {1}", start + 1, (int)end);
            GetDataSource("Load", "");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (start < end - 1)
                start++;
            this.count.Text = String.Format("{0}  из {1}", start + 1, (int)end);
            GetDataSource("Load", "");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClientsAddEdit form = new ClientsAddEdit(null);
            form.Show();
        }

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

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Фамилия";
                textBox1.ForeColor = Color.Gray;
            }
        }

        private void textBox1_MouseEnter(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox1.Text == "Фамилия")
            {
                textBox1.ForeColor = Color.Empty;
                textBox1.Clear();
            }
        }

        private void textBox2_MouseLeave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "Имя";
                textBox2.ForeColor = Color.Gray;
            }
        }

        private void textBox2_MouseEnter(object sender, EventArgs e)
        {
            if (textBox2.Text == "" || textBox2.Text == "Имя")
            {
                textBox2.ForeColor = Color.Empty;
                textBox2.Clear();
            }
        }

        private void textBox3_MouseLeave(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                textBox3.Text = "Отчество";
                textBox3.ForeColor = Color.Gray;
            }
        }

        private void textBox3_MouseEnter(object sender, EventArgs e)
        {
            if (textBox3.Text == "" || textBox3.Text == "Отчество")
            {
                textBox3.ForeColor = Color.Empty;
                textBox3.Clear();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text != "Фамилия" && textBox1.Text != "")
                GetDataSource("SName", textBox1.Text);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text != "Имя" && textBox2.Text != "")
                GetDataSource("Name", textBox2.Text);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text != "Отчество" && textBox3.Text != "")
                GetDataSource("Patro", textBox3.Text);
        }
    }
}
