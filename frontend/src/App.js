import React from 'react';
import './App.css';
import Navbar from "./components/navbar/Navbar";
import User from "./components/UsersPage/User";
import SignInUp from "./components/RegisterPage/SignInUp";
import { BrowserRouter as Router, Switch, Route} from 'react-router-dom';
import Reservation from './components/Rezervari/Reservation'

function App() {
  return (
    <Router>
        <div className="App">
            <Navbar />
            <Switch>
                <Route path='/' exact component={Reservation} />
                <Route path='/rezervari' exact component={Reservation}/>
                <Route path='/users' exact component={User}/>
                <Route path='/register' exact component={SignInUp} />
            </Switch>
        </div>
    </Router>
  );
}

export default App;
