using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EntityFrameworkCrudApp
{
    public partial class EntityFrameworkCrudApp : Form
    {
        Customer model = new Customer();
        public EntityFrameworkCrudApp()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void EntityFrameworkCrudApp_Load(object sender, EventArgs e)
        {
            Clear();
            PopulateDataGridview();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }
        void Clear()
        {
            textFirstname.Text = textLastname.Text = textCity.Text = textAddress.Text = "";
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
            model.Id = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            model.FirstName = textFirstname.Text.Trim();
            model.LastName = textLastname.Text.Trim();
            model.City = textCity.Text.Trim();
            model.Address = textAddress.Text.Trim();
            using (DBEntities db = new DBEntities())
            {
                if (model.Id == 0)//Insert
                {
                    db.Customers.Add(model);
                }
                else //Update
                {
                    db.Entry(model).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
            Clear();
            PopulateDataGridview();
            MessageBox.Show("Information Added Successfully");
        }
        void PopulateDataGridview()
        {
            using (DBEntities db = new DBEntities())
            {
                dgvCustomer.DataSource = db.Customers.ToList<Customer>();
            }
        }

        private void dgvCustomer_DoubleClick(object sender, EventArgs e)
        {
            if(dgvCustomer.CurrentRow.Index != -1)
            {
                model.Id = Convert.ToInt32(dgvCustomer.CurrentRow.Cells["Id"].Value);
                using(DBEntities db = new DBEntities())
                {
                    model = db.Customers.Where(x => x.Id == model.Id).FirstOrDefault();
                    textFirstname.Text = model.FirstName;
                    textLastname.Text = model.LastName;
                    textCity.Text = model.City;
                    textAddress.Text = model.Address;
                }
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you share to Delete this Record?","EF CRUD Operation", MessageBoxButtons.YesNo)== DialogResult.Yes)
            {
                using (DBEntities db = new DBEntities())
                {
                    var entry = db.Entry(model);
                    if(entry.State == EntityState.Detached)
                    {
                        db.Customers.Attach(model);
           
                    }
                    db.Customers.Remove(model);
                    db.SaveChanges();
                    PopulateDataGridview();
                    Clear();
                    MessageBox.Show("Information Deleted Succusfully");
                }
            }
        }
    }
}
