using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Automation.Peers;

namespace WPF_AccessibleItemsControl
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Add some test items.
            List<Bird> items = new List<Bird>();
            items.Add(new Bird() { Name = "House Sparrow", Habitat = "Cities, suburbs, farms", Voice = "Repeated series of chirrup sounds" });
            items.Add(new Bird() { Name = "Golden-crowned Sparrow", Habitat = "Brushy places, including neighborhoods", Voice = "Series of long, raspy, whistled notes" });
            items.Add(new Bird() { Name = "Song Sparrow", Habitat = "Found throughout Puget Sound Region, up to mountain passes", Voice = "Song begins with several clear notes, followed by lower note, jumbled trill" });
            BirdList.ItemsSource = items;
        }
    }

    public class Bird
    {
        public string Name { get; set; }
        public string Habitat { get; set; }
        public string Voice { get; set; }
    }

    public sealed class AccessibleItemsControl : ItemsControl
    {
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new AccessibleItemsControlAutomationPeer(this);
        }

        private sealed class AccessibleItemsControlAutomationPeer : ItemsControlAutomationPeer
        {
            public AccessibleItemsControlAutomationPeer(ItemsControl owner)
                : base(owner)
            {
            }

            protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
            {
                return new AccessibleItemAutomationPeer(item, this);
            }

            protected override string GetClassNameCore() => "AccessibleItemsControl";

            protected override AutomationControlType GetAutomationControlTypeCore() => AutomationControlType.List;
        }

        // The following code's copied from:
        // https://stackoverflow.com/questions/65823779/making-a-grouped-wpf-itemscontrol-accessible-to-screen-readers/

        private sealed class AccessibleItemAutomationPeer : ItemAutomationPeer
        {
            public AccessibleItemAutomationPeer(object item, ItemsControlAutomationPeer itemsControlAutomationPeer)
                : base(item, itemsControlAutomationPeer)
            {
            }

            protected override string GetClassNameCore() => "AccessibleItemsControlItem";
            protected override AutomationControlType GetAutomationControlTypeCore() => AutomationControlType.DataItem;

            // Barker: Add some of my own experiments here...

            // Provide a localized control type which will be announced by a screen reader,
            // rather than having the item said to be a "dataitem".
            protected override string GetLocalizedControlTypeCore()
            {
                return "Bird"; // TODO: Localize this!
            }
        }
    }
}
