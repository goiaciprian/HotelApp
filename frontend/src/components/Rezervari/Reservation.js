import React, {Component} from 'react';
import './rezervariStyle.css'
import DatePicker from "react-date-picker";
import User from "../UsersPage/User";
import Dropdown from "react-dropdown";
import 'react-dropdown/style.css';


function ReservationsStatus(props) {
    const {logged, items, history} = props;
    if(logged) {
        if(items.length > 0) {
            return(
                <ol>
                    {items.map((item, index) => {
                        return(<li key={index}>id: {item.id} nume: {item.userName} tip camera: {item.tipCamera} nr camere: {item.nrCamere} nr persoane: {item.nrPersoane} nr nopti: {item.nrNopti} rezervat pe {item.rezervatPe} pana: {item.rezervatPana} pret fara servicii: {item.pretFaraServicii} pret servicii: {item.totalPretServicii}</li>)
                    })}
                </ol>
            )
        } else {
            return(<p>Nu sunt rezervari facute.</p>)
        }
    } else {
        return (<button className={'butt'} onClick={() => history.push('/register')}>Log in</button> )
    }
}

class Reservation extends Component {
    constructor(props) {
        super(props);
        this.state = {
            rooms: [],
            services: [],
            error: null,
            isLoaded: false,
            reservations: [],
            isLogged: false,
            userID: 0,
            idCameraValue: 0,
            nrCamereValue: 0,
            nrPersoaneValue: 0,
            nrNoptiValue: 0,
            endDate: new Date(),
        };
        this.reservationForm = new React.createRef();
    }


    //form onChange handlers
    handleUserID = (e) => {
        this.setState({userID: e.target.value})
    }
    handleNrCamere = (e) => {
        this.setState( {nrCamereValue: e.target.value});
    }
    handleNrPersoane = (e) => {
        this.setState( {nrPersoaneValue: e.target.value});
    }
    handleDropdown = (e) => {
        this.setState({idCameraValue: e['value']});
    }
    handleNrNopti = (e) => {
        try {
            const value = e.target.value;
            const date = new Date((new Date()).getTime() + (value * 24 * 60 * 60 * 1000))
            this.setState({nrNoptiValue: value, endDate: date});
        } catch {}
    }
    handleEndDate = (value) => {
        try {
            const today = new Date();
            today.setSeconds(0)
            today.setMilliseconds(0)
            value.setSeconds(0)
            value.setMilliseconds(0)
            value.setHours(today.getHours())
            value.setMinutes(today.getMinutes())
            const days = (value.getTime() - today.getTime()) / (1000 * 3600 * 24);
            this.setState({endDate: value, nrNoptiValue: days});
        }catch {}
    }

    findReservations = async () => {
        const auth = 'Bearer ' + localStorage.getItem("TOKEN");
        const options = {
            method: "GET",
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
                'Authorization': auth
            }
        }
        await fetch("http://localhost:8080/api/reservations", options)
            .then(res => {
                if(res.status === 200)
                    this.setState({isLogged: true})
                else if (res.status === 401)
                    this.setState({isLogged: false})
                return res.json()
            })
            .then(response => {
                let updatedResponse = response
                updatedResponse.forEach( (elem) => {
                    elem['rezervatPe'] = new Date(elem['rezervatPe']).toGMTString();
                    elem['rezervatPana'] = new Date(elem['rezervatPana']).toGMTString();
                })
                this.setState({reservations: updatedResponse})
            })
            .catch(error => this.setState({error: error}))
    }

    async componentDidMount() {
        await fetch('http://localhost:8080/api/rooms')
            .then(res=>res.json())
            .then(
                (response) => {
                    this.setState( {
                        isLoaded: true,
                        rooms: response
                    })
                },
                (error) => {
                    this.setState({
                        isLoaded: true,
                        error: error
                    })
                }
            );
        await fetch('http://localhost:8080/api/servicii')
            .then(res=>res.json())
            .then(
                (response) => {
                    this.setState( {
                        isLoaded: true,
                        services: response
                    })
                },
                (error) => {
                    this.setState({
                        isLoaded: true,
                        error: error
                    })
                }
            )
        await this.findReservations();
    }

    displayForm = () => {
        const active = 'active';
        this.reservationForm.current.className += active;
    }

    hideForm = () => {
        const classes = this.reservationForm.current.className;
        this.reservationForm.current.className = classes.replace('active', '');
    }

    makeNewReservation = async () => {

        const newReservation = {
            "nrNopti": parseInt(this.state.nrNoptiValue),
            "nrPersoane": parseInt(this.state.nrPersoaneValue),
            "nrCamere": parseInt(this.state.nrCamereValue),
            "cameraId": parseInt(this.state.idCameraValue),
            "rezervatPana": this.state.endDate.toISOString(),
            "userId": User.isAdmin ? this.state.userID: this.state.reservations[0]['userId']
        }

        const auth = 'Bearer ' + localStorage.getItem("TOKEN");
        const options = {
            method: "POST",
            headers: {
                'Content-Type': 'application/json',
                'Authorization': auth
            },
            body: JSON.stringify(newReservation)
        }

        await fetch("http://localhost:8080/api/reservations", options)
            .then(res => {
                if(res.status === 201) {
                     this.findReservations();
                     this.hideForm();
                }
                return res.json()
            })
            .catch(error => this.setState({error: error}))
    }

    render(){
        const options = [];
        this.state.rooms.forEach((item) => {
            const dropElem = {value: item['id'] , label: item['tipCamera']}
            options.push(dropElem);
        })
        const {services, rooms, isLoaded} = this.state;
        if (!isLoaded)
            return <div className={"center"}><h2>Loading ...</h2></div>
        else
            return (
                <div className={"servicesAndRooms"}>
                    <div className={"services"}>
                        <h3>Servicii</h3>
                        <ol >
                            {services.map( function (elem, index) {
                                return (
                                    <li key={index}>id: {elem['id']} servicu: {elem['numeServiciu']} pret: {elem['pret']}</li>
                                )
                            })}
                        </ol>
                    </div>
                    <div className={'rooms'}>
                        <h3>Camere</h3>
                        <ol >
                            {rooms.map( function (elem, index) {
                                return (
                                    <li key={index}>id: {elem['id']} tip camera: {elem['tipCamera']} camere disponibile: {elem['camereDisponibile']} pret: {elem['pretPeNoapte']}</li>
                                )
                            })}
                        </ol>
                    </div>
                    <div className={'reservationBut'} onClick={this.displayForm}><span>Rezerva</span></div>
                    <div className={'usersReservations'}>
                        <h3>Rezervari facute</h3>
                        <ReservationsStatus logged={this.state.isLogged} history={this.props.history} items={this.state.reservations}/>
                    </div>
                    <div className={'reservationForm '} ref={this.reservationForm} >
                        <div className={'resContainer'}>
                            <form className={'addForm'}>
                                {User.isAdmin &&
                                <label className={'lblf userId'}>User ID <input type={'input'} value={this.state.userID} onChange={this.handleUserID}/></label>
                                }
                                <label className={'lblf roomType'}>Tip camera <Dropdown options={options} onChange={this.handleDropdown} className={'fixWidth'}/></label>
                                <label className={'lblf nrCamere'}>Numar de camere <input type={'input'} value={this.state.nrCamereValue} onChange={this.handleNrCamere}/></label>
                                <label className={'lblf nrPersoane'}>Numar de persoane <input type={'input'} value={this.state.nrPersoaneValue} onChange={this.handleNrPersoane}/></label>
                                <label className={'lblf nrNopti'}>Numar nopti <input type={'input'} value={this.state.nrNoptiValue} onChange={this.handleNrNopti}/></label>
                                <label className={'lblf calendar1'}>Rezervat pana:
                                    <DatePicker value={this.state.endDate} onChange={value => this.handleEndDate(value)} />
                                </label>
                                <button type={'button'} className={'butt'} onClick={this.makeNewReservation}>Rezerva</button>
                            </form>
                            <span className={'closeX'} onClick={this.hideForm}>X</span>
                        </div>
                    </div>
                </div>
            )
    }
}

export default Reservation