using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkingManagement.Lib.DataBase
{
    public class DapperHelper : IDisposable
    {
        OracleConnection _conn;
        OracleTransaction _trans = null;

        public DapperHelper()
        {
            _conn = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=xe)));User Id=ann;Password=111111;");
        }

        public void BeginTransaction()
        {
            if (_conn.State != System.Data.ConnectionState.Open)
                _conn.Open();

            _trans = _conn.BeginTransaction();
        }

        public void Commit()
        {
            _trans.Commit();
            _trans = null;
        }

        public void Rollback()
        {
            _trans.Rollback();
            _trans = null;
        }

        public List<T> Query<T>(string sql, object param)
        {
            return Dapper.SqlMapper.Query<T>(_conn, sql, param, _trans).ToList();
        }
        public T QuerySingle<T>(string sql, object param)
        {
            return Dapper.SqlMapper.QuerySingleOrDefault<T>(_conn, sql, param, _trans);
        }

        public int Execute(string sql, object param)
        {
            //return Dapper.SqlMapper.Execute(_conn, sql, param);
            return Dapper.SqlMapper.Execute(_conn, sql, param, _trans);
        }


        #region Dispose 관련
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리형 상태(관리형 개체)를 삭제합니다.
                    //추가
                    _conn.Dispose();

                    if (_trans != null)
                    {
                        _trans.Rollback();
                        _trans.Dispose();
                    }
                }

                // TODO: 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                disposedValue = true;
            }
        }

        // // TODO: 비관리형 리소스를 해제하는 코드가 'Dispose(bool disposing)'에 포함된 경우에만 종료자를 재정의합니다.
        // ~DapperHelper()
        // {
        //     // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
