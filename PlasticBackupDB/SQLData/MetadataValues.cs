using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlasticBackupDB.SQLData
{
    public class MetadataValues
    {
        // ------   MetadataValues Table  ----------

        public class MetadataItemRow
        {
            public int id = -1;
            public int instanceid = -1;
            string metakey;
            string metavalue;
            public bool error = true; // This class has invalid information.
        }


        public List<MetadataItemRow> getMetaItems(MetadataInstances.MetaInstanceRow instance) { return null; }

        public List<MetadataItemRow> findMetaItemsByKeyValue(string key, string value) { return null; }
    }
}
