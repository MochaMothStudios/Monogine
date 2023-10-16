using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Presenters;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MochaMothMedia.MochaMaker.AvaloniaUI.Factories;
using MochaMothMedia.MochaMaker.Core;
using MochaMothMedia.MochaMaker.Core.UI;
using MochaMothMedia.MochaMaker.Core.UI.Factories.Panes;
using MochaMothMedia.MochaMaker.Editor;
using MochaMothMedia.MochaMaker.YAMLSerializer;
using MochaMothMedia.MochaMaker.Serialization;
using MochaMothMedia.MochaMaker.Core.UI.Factories.Components;
using MochaMothMedia.MochaMaker.Core.UI.Drawables.Windows;
using MochaMothMedia.MochaMaker.Core.UI.Factories;

namespace MochaMothMedia.MochaMaker.AvaloniaLauncher
{
    public partial class App : Application
	{
		IEditorWindow? _editorWindow;

		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);
		}

		public override void OnFrameworkInitializationCompleted()
		{
			ServiceProvider serviceProvider = new ServiceCollection()
				// Component Factories
				.AddSingleton<ILabelFactory, LabelFactory>()
				.AddSingleton<IComponentPaneFactory, ComponentPaneFactory>()
				.AddSingleton<ISplitPaneFactory, SplitPaneFactory>()
				.AddSingleton<ITabbedPaneFactory, TabbedPaneFactory>()

				// Factory Facade
				.AddSingleton<IComponentFactory, ComponentFactoryFacade>()

				// Serialization
				.AddSingleton<ISerializationTools, SerializationTools>()
				.AddSingleton<ILayoutSerializer, LayoutSerializer>()

				// Windows
				.AddSingleton<IEditorWindow, EditorWindow>()

				.BuildServiceProvider();

			_editorWindow = serviceProvider.GetService<IEditorWindow>()!;

			MainWindow mainWindow = new MainWindow();
			mainWindow.Closing += OnClose;
			ContentPresenter presenter = mainWindow.FindControl<ContentPresenter>("content")!;

			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				desktop.MainWindow = mainWindow;
			}

			base.OnFrameworkInitializationCompleted();

			presenter.Content = _editorWindow.GetRoot();
		}

		public void OnClose(object? sender, WindowClosingEventArgs eventArgs)
		{
			_editorWindow?.OnClose();
		}
	}
}
