const axios = require('axios');
(async () => {
    const x = await axios.get('http://127.0.0.1:3000/with-cookie');
    console.log(x.headers);
    const y = await axios.get('http://127.0.0.1:3000/with-cookie');
    console.log(y.headers);
})();