using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
using Windows.UI.Xaml.Data;
using QnStorageClient.Utils;

namespace QnStorageClient.Converters
{
    public class FileMetaDataConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter == null)
            {
                return value;
            }

            if (parameter.ToString() == "date")
            {
                string unixTime = value.ToString();
                unixTime = unixTime.Substring(0, unixTime.Length - 7);

                var startDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var dateTime = startDate.AddSeconds(long.Parse(unixTime)).ToLocalTime();

                return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }

            if (parameter.ToString() == "size")
            {
                long fileSize = long.Parse(value.ToString());

                if (fileSize < 1000)
                {
                    return $"{fileSize} Byte";
                }

                var kbSize = Math.Round(fileSize / 1024.0, 2);
                if (kbSize < 1000)
                {
                    return $"{kbSize} Kb";
                }

                return $"{Math.Round(fileSize / 1024.0 / 1024.0, 2)} M";
            }

            if (parameter.ToString() == "type")
            {
                int fileType = int.Parse(value.ToString());
                return fileType == 1 ? ResourceUtils.GetText("LowStorage"): ResourceUtils.GetText("HightStorage");
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
