import logo from './logo.svg';
import './App.css';
import {post} from 'axios'

const onLogin = async () => {
  await post("https://localhost:7209/authentication/signin/google")
}

function App() {
  return (
    <button onClick={onLogin}>Login Google</button>
  );
}

export default App;
