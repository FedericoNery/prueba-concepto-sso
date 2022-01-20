import { GoogleLogout } from 'react-google-login';
import { useNavigate } from 'react-router-dom';

const {REACT_APP_CLIENT_ID}= process.env

const Home = () => {
    const navigate = useNavigate()
    const onLogoutSuccess = () => {
        navigate("/")
    }

    const onFailure = async (res) => {
        console.log("status: " + res.error)
        console.log("details: " + res.details)
    };

    return <>
        <GoogleLogout
            clientId={REACT_APP_CLIENT_ID}
            buttonText="Logout"
            onLogoutSuccess={onLogoutSuccess}
            onFailure={onFailure}
        >
        </GoogleLogout>
    </>
}

export default Home