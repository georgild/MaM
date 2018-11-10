
import React from 'react';
import Modal from 'react-modal';
import {PropTypes} from 'prop-types';
import $ from 'jquery';

const customStyles = {
    content : {
      top                   : '50%',
      left                  : '50%',
      right                 : 'auto',
      bottom                : 'auto',
      marginRight           : '-50%',
      transform             : 'translate(-50%, -50%)'
    }
  };

class OrderDialog extends React.Component {

    constructor(props) {
        super(props);
        this.state = { 
            email: '',
            seatClassMap : []
        };
    }

    static propTypes = {
        selectedRowId: PropTypes.string,
        dialogIsOpen: PropTypes.bool,
        onCloseClick: PropTypes.func,
        rowsCount: PropTypes.number,
        colsCount: PropTypes.number

    }

    closeModal = (e) => {
        e.preventDefault();
        this.props.onCloseClick();
    }

    onSubmitOrder = (e) => {
        e.preventDefault();

        var email = this.state.email.trim(),
            selectedSeats = [],
            self = this;

        this.state.seatClassMap.forEach(function(row, i){ 

            row.forEach(function(cell, j){ 
                if (self.state.seatClassMap[i][j] === 'selected') {
                    selectedSeats.push({
                        Row: i,
                        Col: j
                    });
                }
            });
        });
        this.props.onSubmitOrder(email, selectedSeats);
    }

    onAfterOpen = () => {
        this.clearPreviousState();
        this.loadReservedSeats();
    }

    loadReservedSeats() {
        $.ajax({
            url: '/api/v1/orders/routes/' + this.props.selectedRowId + '/reservedseats',
            data: '',
            dataType: 'json',
            cache: false,
            success: function (data) {
                if (data) {
                    var seatClassMap = this.state.seatClassMap;
                    data.forEach(function(item, i){   
                        seatClassMap[item.Row][item.Col] = 'reserved';
                    });
                    this.setState({ seatClassMap: seatClassMap })
                }
            }.bind(this),
            error: function (xhr, status, err) {
                console.error(this.props.url, status, err.toString());
            }.bind(this)
        });
    }

    clearPreviousState = () => {
        var seatClassMap = [];

        for (var i =0; i < this.props.rowsCount; i++) {

            seatClassMap[i] = [];
            //this.setState({ seatClassMap: arrayvar })

            for (var j=0; j < this.props.colsCount; j++) {
                seatClassMap[i][j] = '';
            }
        }

        this.setState({ seatClassMap: seatClassMap })
    }

    handleSeatClick = (e) => {
        e.preventDefault();

        var seatClassMap = this.state.seatClassMap;

        switch (seatClassMap[e.target.parentNode.rowIndex][e.target.cellIndex]) {
            case 'selected': 
                seatClassMap[e.target.parentNode.rowIndex][e.target.cellIndex] = '';
                break;
            case '': 
                seatClassMap[e.target.parentNode.rowIndex][e.target.cellIndex] = 'selected';
                break;
            default:
                break;
        }
        
        this.setState({ seatClassMap: seatClassMap });
    }

    handleEmailChange = (e) => {
        this.setState({ email: e.target.value });
    }

    componentWillMount = () => {
        this.clearPreviousState();
    }

    render() {
        var rows = new Array(this.props.rowsCount),
            cols = new Array(this.props.colsCount);
        return (
            <Modal
                    isOpen={this.props.dialogIsOpen}
                    onAfterOpen={this.onAfterOpen}
                    onRequestClose={this.closeModal}
                    style={customStyles}
                    contentLabel="Example Modal"
                    ariaHideApp={false}
                >
                <p>Order ticket</p>
                <form onSubmit={this.handleSubmit}>
                    <label htmlFor="E-Mail">E-Mail:</label>
                    <input
                        id="E-Mail"
                        type="email"
                        value={this.state.email}
                        onChange={this.handleEmailChange}
                    />
                    <br></br>
                    <label htmlFor="Phone">Phone:</label>
                    <input
                        id="Phone"
                        type="text"
                    />
                    <br></br>
                </form>
                <table className="ticket-table">
                    <tbody>
                        
                        {Array.apply(null, rows).map(function(item, i){                                        
                            return (
                                <tr key={i} >
                                    {Array.apply(null, cols).map(function(item, j){                                        
                                        return (
                                            <td key={i+j} onClick={this.handleSeatClick} className={this.state.seatClassMap[i][j]}></td>
                                        );                
                                    }, this)} 
                                </tr>
                            );
                        }, this)} 
                    </tbody>
                </table> 
                <input className="button" onClick={this.onSubmitOrder} type="submit" value="Order" />
                <button className="button" onClick={this.closeModal}>close</button>
            </Modal>
        );
    }
}

export default OrderDialog;