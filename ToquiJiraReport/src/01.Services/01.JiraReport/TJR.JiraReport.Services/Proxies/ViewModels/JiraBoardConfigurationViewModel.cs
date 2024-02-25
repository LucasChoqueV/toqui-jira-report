using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TJR.JiraReport.Services.Proxies.ViewModels
{
    public class JiraBoardConfigurationViewModel
    {
        public ProxyColumnConfiguration ColumnConfig { get; set; }
    }

    public class ProxyColumnConfiguration
    {
        public IEnumerable<ProxyColumn> Columns { get; set; }
    }

    public class ProxyColumn
    {
        public string Name { get; set; }
    }
}
