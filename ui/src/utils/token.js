const token = {
    isExpired: function () {
        if(localStorage.expiration)
        {
            var expirationDt = new Date(localStorage.expiration)
            if (expirationDt.getTime() < (new Date()).getTime()) {
                return true
            }
        }
        return false
    },
    clear: function () {      
        localStorage.removeItem('token')
        localStorage.removeItem('expiration')
    },
    set: function(token) {
        localStorage.setItem('token', token)
        localStorage.setItem('expiration', new Date((new Date()).getTime() + 20*60000))
    },
    hasValue: function () {
        if(localStorage.token)
            return true
        else 
            return false
    },
    obtain: function () {
        if (localStorage.token){
            return localStorage.token
        }
        return ''
    }
} 
export default token