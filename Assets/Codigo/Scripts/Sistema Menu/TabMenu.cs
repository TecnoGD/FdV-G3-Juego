
namespace Codigo.Scripts.Sistema_Menu
{
    public class TabMenu : Menu
    {
        public Menu currentShownTab = null;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        
        public void MostrarTab(Menu tab)
        {
            if(currentShownTab)
                currentShownTab.gameObject.SetActive(false);
            
            currentShownTab = tab;
            tab.gameObject.SetActive(true);
        }
    }
}