import React from 'react';
import $ from 'jquery';
import {PropTypes} from 'prop-types';

import {Grid, Row, Col, Thumbnail, Tabs, Tab, Table} from 'react-bootstrap'

import Tree, { TreeNode } from 'rc-tree';
import 'rc-tree/assets/index.css';

import styled from 'styled-components';

import ReactDataGrid from 'react-data-grid';
import FilterForm from './FilterForm';

class Folders extends React.Component {

    constructor(props) {
        super(props);
        this.state = { 
            data: [], 
            filters : [],
            assets: [{
                id: "abc",
                title: "abc",
                description: "abc",
                altText: "abc",
                src: "abc",
                height: 100,
                width: 100,
                thumbSrc: "abc",
                thumbHeight: 111,
                thumbWidth: 111
              }],
            treeData: [
                { key: '0-0', title: 'parent 1', children:
                  [
                    { key: '0-0-0', title: 'parent 1-1', children:
                      [
                        { key: '0-0-0-0', title: 'parent 1-1-0' },
                      ],
                    },
                    { key: '0-0-1', title: 'parent 1-2', children:
                        [
                          { key: '0-0-1-0', title: 'parent 1-2-0', disableCheckbox: true },
                          { key: '0-0-1-1', title: 'parent 1-2-1' },
                        ],
                    },
                  ],
                },
              ]
        };
    }

    static propTypes = {
        url: PropTypes.string,
        //initialFilters: PropTypes.JSON,
        pollInterval: PropTypes.number
    }

    columns = [{ 
        key: 'ArrivesAt', 
        name: 'Arrives At' 
    }, { 
        key: 'ArrivesFrom', 
        name: 'Arrives From' 
    }, { 
        key: 'CompanyName', 
        name: 'Company Name' 
    }, { 
        key: 'TicketPrice', 
        name: 'Ticket Price' 
    }]

    loadArrivals(filters, currency) {
        var self = this;
        $.ajax({
            url: this.props.url,
            data: 'filters=' + JSON.stringify(this.props.initialFilters.concat(filters)) + '&currency=' + currency,
            dataType: 'json',
            async: true,
            cache: false,
            success: function (data) {
                var parsedData = [];

                if (data) {
                    data.forEach(function(route) {
                        parsedData.push({
                            ArrivesAt: (new Date(route.FinalStop.ArrivalDate)).toLocaleString(),
                            ArrivesFrom: route.InitialStop.City,
                            CompanyName: route.CompanyName,
                            TicketPrice: route.TicketPrice + ' ' + self.state.currency
                        });
                    });
                }
                
                this.setState({ data: parsedData });
            }.bind(this),
            error: function (xhr, status, err) {
                console.error(this.props.url, status, err.toString());
            }.bind(this)
        });
    }

    handleFiltersSubmit = (filters, currency) => {
        this.setState({ filters: filters });
        this.setState({ currency: currency });
        this.loadArrivals(filters, currency);
    }

    componentDidMount = () => {
        this.loadArrivals(this.state.filters, this.state.currency);
        //setInterval(this.loadArrivals, this.props.pollInterval);
        // <FilterForm onFilterSubmit={this.handleFiltersSubmit}/>
    }

    render() {
        return (
            <span>
                
                <Tree
                    defaultExpandAll={true}
                    defaultExpandedKeys={['p1']}
                    //openAnimation={animation}
                    treeData={this.state.treeData}
                >
                </Tree>
                
                <Grid>
                <Row>
                  <Col xs={6} md={3}>
                    <Thumbnail href="#" alt="171x180" src="/favicon.ico" />
                    <p>Asset 1</p>
                  </Col>
                  <Col xs={6} md={3}>
                    <Thumbnail href="#" alt="171x180" src="/favicon.ico" />
                    <p>Asset 1</p>
                  </Col>
                  <Col xs={6} md={3}>
                    <Thumbnail href="#" alt="171x180" src="/favicon.ico" />
                    <p>Asset 1</p>
                  </Col>
                </Row>
                <Row>
                  <Col xs={6} md={3}>
                    <Thumbnail href="#" alt="171x180" src="/favicon.ico" />
                    <p>Asset 1</p>
                  </Col>
                  <Col xs={6} md={3}>
                    <Thumbnail href="#" alt="171x180" src="/favicon.ico" />
                    <p>Asset 1</p>
                  </Col>
                  <Col xs={6} md={3}>
                    <Thumbnail href="#" alt="171x180" src="/favicon.ico" />
                    <p>Asset 1</p>
                  </Col>
                </Row>
              </Grid>;
              <Tabs defaultActiveKey={2} id="uncontrolled-tab-example">
                <Tab eventKey={1} title="Metadata">
                    <Table striped bordered condensed hover>
                    <thead>
                        <tr>
                        <th>#</th>
                        <th>First Name</th>
                        <th>Last Name</th>
                        <th>Username</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                        <td>1</td>
                        <td>Mark</td>
                        <td>Otto</td>
                        <td>@mdo</td>
                        </tr>
                        <tr>
                        <td>2</td>
                        <td>Jacob</td>
                        <td>Thornton</td>
                        <td>@fat</td>
                        </tr>
                        <tr>
                        <td>3</td>
                        <td colSpan="2">Larry the Bird</td>
                        <td>@twitter</td>
                        </tr>
                    </tbody>
                    </Table>;
                </Tab>
                <Tab eventKey={2} title="Tasks">
                    Tab 2 content
                </Tab>
                <Tab eventKey={3} title="Analytics">
                    Tab 3 content
                </Tab>
            </Tabs>;
            </span>
        );
    }
}

export default Folders;