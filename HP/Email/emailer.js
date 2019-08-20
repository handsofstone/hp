const sql = require('mssql')
const mjmlutils = require('mjml-utils')

var config = {
    user:'nlpool',
    password:'nlpool',
    server:'10.185.102.80',
    database:'nlpool'
}

var pool = new sql.ConnectionPool(config)

pool.connect()

var request = new sql.Request(pool)

request.query()
pool.close()