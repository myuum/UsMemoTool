using System;
using System.Windows.Input;

namespace UsMemoTool.Utility
{
    public class DelegateCommand : ICommand
    {
        //! コマンドを実行するためのメソッド
        private Action<object> execute;

        //! コマンドの実行可否を判別するメソッド
        private Func<object, bool> canExecute;

        /**
         * @brief コンストラクタ
         * 
         * @param [in] execute コマンドを実行するためのメソッド
         */
        public DelegateCommand(Action<object> execute) : this(execute, o => true)
        {

        }

        /**
         * @brief コンストラクタ
         * 
         * @param [in] execute コマンドを実行するためのメソッド
         * @param [in] canExecute コマンドの実行可否を判別するメソッド
         */
        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /**
         * @brief コマンドの実行可否を判別します。
         * 
         * @param [in] パラメータ
         * @return 判別結果(true:実行可能/false:実行不可)
         */
        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute(parameter);
        }

        /**
         * @brief コマンドを実行します。
         * 
         * @param [in] parameter パラメータ
         */
        public void Execute(object parameter)
        {
            // コマンドを実行するメソッドが無効な場合
            if (execute == null)
            {
                // 何もしない。
                return;
            }

            // コマンドを実行します。
            execute(parameter);
        }

        //! コマンドを実行するかどうかに影響するような変更があった場合に発生します。
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public static void RaiseCanExecuteChanged()
        {
            // RequerySuggestedイベントを強制的に発生させます。
            CommandManager.InvalidateRequerySuggested();
        }
    }
}   