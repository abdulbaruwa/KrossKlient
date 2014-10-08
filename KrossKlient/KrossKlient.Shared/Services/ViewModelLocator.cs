using Windows.ApplicationModel;
using KrossKlient.ViewModels;
using ReactiveUI;
using Splat;

namespace KrossKlient.Services
{
    public class ViewModelLocator
    {

        public ViewModelLocator()
        {
            if (DesignMode.DesignModeEnabled) return;
            Resolver = Locator.CurrentMutable;
            
            //Resolver.RegisterConstant(UserService()); RegisterConstant(IUserService,UserService>());
            //Resolver.Default.Register<IPuzzleRepository, PuzzleRepository>();
            //SimpleIoc.Default.Register<IPuzzlesService, PuzzlesService>();
            //SimpleIoc.Default.Register<IPuzzleWebApiService, PuzzleWebApiService>();
            //SimpleIoc.Default.Register<IGameDataService, GameDataService.GameDataService>();

        }

        public HomePageViewModel PuzzlesViewModel
        {
            get
            {
                return new HomePageViewModel(Resolver.GetService<IPuzzleRepository>(), Resolver.GetService<IUserService>());
            }
        }
        public IMutableDependencyResolver Resolver { get; set; }