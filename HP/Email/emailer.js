var sql = require('mssql')

var config = {
    user:'nlpool',
    password:'nlpool',
    server:'10.185.102.80',
    database:'nlpool'
}

var pool = new sql.ConnectionPool(config)

pool.connect()

pool.close()