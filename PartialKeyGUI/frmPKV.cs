using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using PartialKeyVerification;
using PartialKeyVerification.Checksum;
using PartialKeyVerification.Hash;

namespace PartialKeyGUI
{
    public partial class frmPKV : Form
    {
        private enum pkvChecksum
        {
            Adler16,
            Crc16,
            CrcCcitt
        }

        private enum pkvHash
        {
            Crc32,
            Fnv1A,
            GeneralizedCrc,
            Jenkins06,
            Jenkins96,
            OnaAtATime,
            SuperFast
        }

        public frmPKV()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var generator = new PartialKeyGenerator(new Adler16(), new Jenkins96(), new uint[] { 1, 2 }) { Spacing = 5 };
            var key = generator.Generate("henk");

            Console.WriteLine("{0}", key.ToString());

            var isValid = PartialKeyValidator.ValidateKey(new Adler16(), new Jenkins96(), key, 0, 1);

            Console.WriteLine("{0}", isValid);

            IChecksum16 objChecksum;
            IHash objHash;

            switch (cbxChecksum.SelectedIndex)
            {
                case 0:
                    objChecksum = new Adler16();
                    break;
                case 1:
                    objChecksum = new Crc16();
                    break;
                case 2:
                    objChecksum = new CrcCcitt();
                    break;
                default:
                    objChecksum = new Adler16();
                    break;
            }

            switch (cbxHash.SelectedIndex)
            {
                case 0:
                    objHash = new Crc32();
                    break;
                case 1:
                    objHash = new Fnv1A();
                    break;
                case 2:
                    objHash = new GeneralizedCrc();
                    break;
                case 3:
                    objHash = new Jenkins06(0);
                    break;
                case 4:
                    objHash = new Jenkins96();
                    break;
                case 5:
                    objHash = new OneAtATime();
                    break;
                case 6:
                    objHash = new SuperFast();
                    break;
                default:
                    objHash = new Jenkins96();
                    break;
            }

            uint[] uInt = new uint[] { };
            int intKeySize = (int)nudSubKeys.Value;            
            List<uint> list = new List<uint>();

            for (uint k=0; k<intKeySize; k++)
            {
                list.Add(k+1);
            }

            uInt = list.ToArray();

            generator = new PartialKeyGenerator(objChecksum, objHash, uInt) { Spacing = (byte)nudSpacing.Value };
            key = generator.Generate(txtSeed.Text);

            txtKey.Text = key;

            dataGridView1.Rows.Add(
                cbxChecksum.SelectedItem,
                cbxHash.SelectedItem,
                txtSeed.Text,
                nudSubKeys.Value.ToString(),
                txtKey.Text);
            dataGridView1.AutoResizeColumns();
        }

        private void frmPKV_Load(object sender, EventArgs e)
        {
            foreach (string s in Enum.GetNames(typeof(pkvChecksum)))
            {
                cbxChecksum.Items.Add(s);
            }
            
            foreach (string s in Enum.GetNames(typeof(pkvHash)))
            {
                cbxHash.Items.Add(s);
            }

            cbxChecksum.SelectedIndex = 0;
            cbxHash.SelectedIndex = 4;

            nudSpacing.Value = 5;
            nudSubKeys.Value = 2;

            txtSeed.Text = "bob@smith.com";

            dataGridView1.Columns.Add( "Checksum", "Checksum" );
            dataGridView1.Columns.Add("Hash", "Hash");
            dataGridView1.Columns.Add("Seed", "Seed");
            dataGridView1.Columns.Add("Subkeys", "Subkeys");
            dataGridView1.Columns.Add("Key", "Key");            
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowIndex = dataGridView1.SelectedCells[0].RowIndex;
            string key = dataGridView1.Rows[rowIndex].Cells["Key"].Value.ToString();

            Clipboard.SetText(key);
        }

        private void deleteRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.RemoveAt(row.Index);
            }
        }

        private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }
    }
}
