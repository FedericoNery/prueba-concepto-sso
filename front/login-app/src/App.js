import './App.css';
import { post, get } from 'axios'
import { Route, Routes, useNavigate } from 'react-router-dom'
import { useState } from 'react';
import Home from './Home';
import GoogleLogin from 'react-google-login';

const config = {
  headers: {
    'Content-Type': 'application/json',
    //'X-Requested-With': 'XMLHttpRequest',
    "Access-Control-Allow-Origin": '*'
  }
};

const {REACT_APP_CLIENT_ID}= process.env

function App() {
  const navigate = useNavigate()
  const [isAuth, setIsAuth] = useState(false)
  const [data, setData] = useState(null)

  const onLogin = async () => {

    const result = await post("https://localhost:7209/api/Authentication/signin/google", config)
    console.log(result)
    //setIsAuth(true)
  }

  const signIn = async () => {
    const redirectUri = "http://localhost:3000/home"
    window.location.href = "https://accounts.google.com/o/oauth2/v2/auth?" +
      'scope=openid%20email' +
      '&response_type=token' +
      '&redirect_uri=' + redirectUri +
      '&client_id=' + REACT_APP_CLIENT_ID

    if (sessionStorage.getItem("access_token") == null) {

      if (window.location.hash) {
        if (window.location.hash.split("access_token")) {
          var access_token = window.location.hash.split("access_token")[1].split('&')[0]
          //sessionStorage.setItem("access_token", access_token)

          const url = "https://www.googleapis.com.oauth2/v1/userinfo?alt=json&access_token=" + access_token
          const data = await get(url)
          //sessionStorage.setItem("userName", data.email)
          console.log(data)
        }
      }
      else {
        alert("Access token expirado, login again")
        navigate("/")
      }
    }
  }

  //Esto debería ser lo que responde la API nuestra creo...
  const onSuccess = async (res) => {
    /* try {
      const result = await post("/auth/", {
        token: res?.tokenId,
      });
    } catch (err) {
      console.log(err);
    } */
    const { googleId, tokenId, accessToken, tokenObj, profileObj } = res
    console.log("ACCESS TOKEN")
    console.log(accessToken)
    console.log("TOKEN OBJ")
    console.log(tokenObj)
    console.log("PROFILE OBJ")
    console.log(profileObj)
    setIsAuth(true)
    const { access_token, expires_at, expires_in, token_type, id_token } = tokenObj
    const { email, name, imageUrl } = profileObj
    const result = await post("https://localhost:7209/api/Authentication/signin/google", 
    { 
      idToken: id_token,
      accessToken: access_token, 
      expiresAt: expires_at, 
      expiresIn: expires_in, 
      tokenType: token_type 
    })

    //que result devuelva o alguna forma de obtener el status code después en el interceptor
    navigate("/home")
  };

  const onFailure = async (res) => {
    console.log("status: " + res.error)
    console.log("details: " + res.details)
  };



  return (
    <Routes>
      <Route path="/" element={<>
        <button onClick={onLogin}>Login Google</button>
        <button onClick={signIn}>Sign in Google</button>
        <GoogleLogin
          clientId={REACT_APP_CLIENT_ID}
          buttonText="Login"
          onSuccess={onSuccess}
          onFailure={onFailure}
          cookiePolicy={'single_host_origin'}
        />
      </>} />
      <Route path="/home" element={isAuth &&
        <Home />
      } />
    </Routes>
  );
}

export default App;
