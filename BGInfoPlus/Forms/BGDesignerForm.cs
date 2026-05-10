using BGInfoPlus.Modules.Core;
using BGInfoPlus.Modules.Data;

namespace BGInfoPlus
{
    public partial class BGDesignerForm : Form
    {
        public BGDesignerForm()
        {
            InitializeComponent();
        }

        private void BGDesignerForm_Load(object sender, EventArgs e)
        {
            //Get background image from registry.
            bgSelectedImage.ImageLocation = SystemDataProvider.GetData(new SystemDataProvider.DataRequest
            {
                SourceType = SystemDataProvider.DataSourceType.Registry,
                Query = @"HKEY_CURRENT_USER\Control Panel\Desktop",
                QueryValueName = "WallPaper",
                ExpectedDataType = SystemDataProvider.DataType.String
            }) as String;
            bgSelectedImage.Load();
        }
    }
}
