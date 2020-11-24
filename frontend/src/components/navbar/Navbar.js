import React, { Component } from 'react';
import { MenuItems } from './MenuItems';
import './Navbar.css';
import {Button} from "../Button";
import {Link} from 'react-router-dom';


class Navbar extends Component {
    state = { clicked: false };

    handleClick = () => {
        this.setState({ clicked: !this.state.clicked })
    }

    render() {
        return(
            <nav className="NavbarItems">
                <h1 className='navbar-text'> Hotel App</h1>
                <div className='menu-icon' onClick={this.handleClick}>
                    <i className={this.state.clicked ? 'fas fa-times' : 'fas fa-bars'}/>
                </div>
                <ul className={this.state.clicked ? 'nav-menu active': 'nav-menu'}>
                    {MenuItems.map((item, index) => {
                        return(
                            <Link to={item.redirect}>
                                <li key={index}>
                                    <span key={index} className={item.cName}>
                                        {item.title}
                                    </span>
                                </li>
                            </Link>
                        )
                    })}
                </ul>
                <Link to='/register'>
                    <Button>Sign up</Button>
                </Link>
            </nav>
        )
    }
}

export default Navbar