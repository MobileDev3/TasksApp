using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TasksApp.Helper
{
    class customAdapter: BaseAdapter
    {
        private tasksClass mainTasks;
        private List<string> taskList;
        private DbHelper dbHelper;
        public customAdapter(tasksClass mainTasks, List<string> taskList, DbHelper dbHelper)
        {
            this.mainTasks = mainTasks;
            this.taskList = taskList;
            this.dbHelper = dbHelper;
        }

        public override int Count
        {
            get
            {
                return taskList.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = (LayoutInflater)mainTasks.GetSystemService(Context.LayoutInflaterService);
            View view = inflater.Inflate(Resource.Layout.Row, null);
            TextView txtTask = view.FindViewById<TextView>(Resource.Id.task_title);
            Button btnDelete = view.FindViewById<Button>(Resource.Id.btnDelete);
            txtTask.Text = taskList[position];
            btnDelete.Click += delegate
            {
                string task = taskList[position];
                dbHelper.deleteTask(task);
                mainTasks.LoadTaskList();
            };
            return view;
        }
    

    }
}