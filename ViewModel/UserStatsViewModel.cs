using System.Collections.ObjectModel;
using System.Linq;
using ElFartas.InstantEnglish.DAO;

namespace ViewModel
{
    public class UserStatsViewModel
    {
        public ObservableCollection<string> StatItems { get; set; }

        public UserStatsViewModel()
        {
            var data = DataProvider.GetDataAccess();
            var stats = data.GetStats(LoginViewModel.CurrentUser);
            var items = data.GetItems();
            stats = stats.OrderByDescending(s => s.Correct/(s.Correct + s.Failed + 1));
            var statList = stats.Select(
                stat =>
                    items.First(i => i.Id == stat.ItemId).Name + " " + stat.Correct + "/" + (stat.Correct + stat.Failed));
            StatItems = new ObservableCollection<string>(statList);
        }
    }
}
