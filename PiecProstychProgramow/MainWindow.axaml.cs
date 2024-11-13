using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PiecProstychProgramow;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Zadania.ItemsSource = _filteredTasks;
    }

    private readonly ObservableCollection<TaskItem> _tasks = [];
    private readonly ObservableCollection<TaskItem> _filteredTasks = [];
        
    private void newTask(object? sender, RoutedEventArgs e)
    {
        var task = new TaskItem(Task.Text ?? string.Empty);
        task.UpdateFilter = Filter;
        task.DeleteItem = new Command(() =>
        {
            _tasks.Remove(task);
            Filter();
        });
        _tasks.Add(task);
        Filter();
        Task.Text = string.Empty;
    }

    private void Filter()
    {
        string selectedFilter = "";

        if (FilterComboBox.SelectedItem != null)
        {
            selectedFilter = (FilterComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? string.Empty;
        }

        switch (selectedFilter)
        {
            case "Zrobione":
                FilterTasks(_tasks.Where(t => t.IsCompleted));
                break;
            case "W trakcie":
                FilterTasks(_tasks.Where(t => !t.IsCompleted));
                break;
            default:
                FilterTasks(_tasks);
                break;
            // _filteredTasks.(_tasks);
        }
    }

    private void FilterChanged(object? sender, SelectionChangedEventArgs e)
    {
        Filter();
    }

    private void FilterTasks(IEnumerable<TaskItem> tasks)
    {
        _filteredTasks.Clear();
        foreach (var task in tasks)
        {
            _filteredTasks.Add(task);
        }
    }
}

internal class TaskItem(string description){
    public string Description { get; } = description;
    
    private bool _isCompleted;

    public bool IsCompleted
    {
        get => _isCompleted;
        set
        {
            _isCompleted = value;
            UpdateFilter?.Invoke();
        }
    }

    public ICommand DeleteItem { get; set; }
    public Action? UpdateFilter { get; set; }
}