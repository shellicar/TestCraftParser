using System;
using System.Collections.Immutable;
using System.Linq;
using System.Windows.Forms;
using NordockCraft.Data.Service;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        private Timer typingTimer;

        public Form1()
        {
            InitializeComponent();
            ServiceCreator = new ServiceCreator();

            ResizeViews();
        }


        public ServiceCreator ServiceCreator { get; }

        private void ResizeViews()
        {
            foreach (var view in new[] {listViewIngredients, listViewUses, listViewSimilar, listViewCraftedAt, listViewUsedAt})
            {
                if (view.Items.Count > 0)
                {
                    view.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    view.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (typingTimer == null)
            {
                typingTimer = new Timer {Interval = 1000};
                typingTimer.Tick += HandleTypingTimerTimeout;
            }

            typingTimer.Stop();
            typingTimer.Tag = (sender as TextBox)?.Text;
            typingTimer.Start();
        }

        private void HandleTypingTimerTimeout(object sender, EventArgs e)
        {
            if (!(sender is Timer timer))
            {
                return;
            }

            timer.Stop();

            DoLookup(timer.Tag as string);
        }

        private void DoLookup(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return;
            }

            using (var service = ServiceCreator.LookupService)
            {
                var similarResults = service.LookupSimilar(searchText);
                UpdateSimilarResults(similarResults);

                var result = service.LookupItem(searchText);
                if (result != null)
                {
                    UpdateResults(result);
                }

                ResizeViews();
            }
        }

        private void UpdateSimilarResults(IImmutableList<ItemDto> results)
        {
            listViewSimilar.Items.Clear();

            foreach (var r in results)
            {
                var item = new ListViewItem();
                item.Text = r.Name;
                listViewSimilar.Items.Add(item);
            }
        }

        private void UpdateResults(ItemDto result)
        {
            textBoxCurrentItem.Text = result.Name;

            listViewIngredients.Items.Clear();
            listViewUses.Items.Clear();
            listViewCraftedAt.Items.Clear();
            listViewUsedAt.Items.Clear();

            AddLocationsToView(listViewCraftedAt, result.CreatedAtLocations);
            AddLocationsToView(listViewUsedAt, result.UsedAtLocations);

            if (result.CreatedBy != null)
            {
                foreach (var q in result.CreatedBy.Ingredients)
                {
                    var item = new ListViewItem {Text = q.ItemName};
                    item.SubItems.Add(q.Amount.ToString());
                    listViewIngredients.Items.Add(item);
                }
            }

            foreach (var q in result.UsedIn)
            {
                var item = new ListViewItem {Text = q.ItemCreated};
                listViewUses.Items.Add(item);
            }
        }

        private static void AddLocationsToView(ListView destList, IImmutableList<LocationDto> src)
        {
            destList.Items.AddRange(src.Select(x => new ListViewItem {Text = x.Name}).ToArray());
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!(sender is ListView view))
            {
                return;
            }
            if (view.SelectedItems.Count > 0)
            {
                var item = view.SelectedItems[0];
                var text = item.Text;
                DoLookup(text);
            }
        }
    }
}