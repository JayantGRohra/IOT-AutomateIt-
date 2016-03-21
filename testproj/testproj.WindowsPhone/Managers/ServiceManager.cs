using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testproj.Managers
{
    static class ServiceManager
    {
        public static async Task<bool> TryInsertRitualAsync(IMobileServiceTable<Ritual> ritualTable, Ritual ritual)
        {
            try
            {
                await ritualTable.InsertAsync(ritual);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> TryDeleteRitualAsync(IMobileServiceTable<Ritual> ritualTable,Ritual ritual)
        {
            try
            {
                await ritualTable.DeleteAsync(ritual);
                return true;

            }
            catch
            {
                return false;
            }
        }

        public static async Task<MobileServiceCollection<Ritual, Ritual>> SelectAllAsync(IMobileServiceTable<Ritual> ritualTable)
        {
            try
            {
                return await ritualTable.ToCollectionAsync<Ritual>();

            }
            catch
            {
                return null;
            }
        }

    }
}
