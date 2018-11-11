import React from 'react';
import $ from 'jquery';
import {PropTypes} from 'prop-types';

import {Grid, Row, Col, Thumbnail, Tabs, Tab, Table, FormGroup, ControlLabel, FormControl, HelpBlock, Button} from 'react-bootstrap'

import Tree, { TreeNode } from 'rc-tree';
import 'rc-tree/assets/index.css';

class Folders extends React.Component {

    constructor(props) {
        super(props);
        this.onToggle = this.onToggle.bind(this);
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
                { key: '0-0', title: 'My Organization', children:
                [
                    { key: '0-0-0', title: 'Movies', children: [], isLeaf: false
                    },
                    { key: '0-0-1', title: 'Pictures', children: [], isLeaf: false
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
    onToggle(node, toggled){
        if(this.state.cursor){this.state.cursor.active = false;}
        node.active = true;
        if(node.children){ node.toggled = toggled; }
        this.setState({ cursor: node });
    }
   /*<form>
    <FormGroup
    //controlId="formBasicText"
    //validationState={this.getValidationState()}
    >
    <ControlLabel>Search</ControlLabel>
    <FormControl
        type="text"
        value={this.state.value}
        placeholder="Enter text"
        onChange={this.handleChange}
    />
    <FormControl.Feedback />
    </FormGroup>
</form>*/
    render() {
        return (
            <div>
    
                 <Table striped bordered condensed hover responsive>
                    <thead>
                        <tr>
                            <th>Folders</th>
                            <th>Assets</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>
                                <Tree
                                    defaultExpandAll={true}
                                    defaultExpandedKeys={['p1']}
                                    //openAnimation={animation}
                                    treeData={this.state.treeData}
                                >
                                </Tree>
                            </td>
                            <td>
                            
                                <Grid>
                                        <Row>
                                            <Col xs={3} md={2}>
                                                <Thumbnail href="#" alt="171x180" src="pics/avengers.png" >
                                                    <p>Avengers</p>
                                                </Thumbnail>
                                            </Col>
                                            <Col xs={3} md={2}>
                                                <Thumbnail href="#" alt="171x180" src="pics/first_blood.jpg" >
                                                    <p>First Blood</p>
                                                </Thumbnail>
                                            </Col>
                                            <Col xs={3} md={2}>
                                                <Thumbnail href="#" alt="171x180" src="pics/the_lord.jpg" >
                                                    <p>LOTR</p>
                                                </Thumbnail>
                                            </Col>
                     
                                            <Col xs={3} md={2}>
                                                <Thumbnail href="#" alt="171x180" src="pics/venom.jpg" >
                                                    <p>Venom</p>
                                                </Thumbnail>
                                            </Col>
                                            <Col xs={3} md={2}>
                                                <Thumbnail href="#" alt="171x180" src="pics/spider_man.jpg" >
                                                    <p>Spider Man</p>
                                                </Thumbnail>
                                            </Col>
                                        </Row>

                                </Grid>
                                <Tabs defaultActiveKey={1} id="uncontrolled-tab-example">
                                    <Tab eventKey={1} title="Metadata">
                                        <Table striped bordered condensed hover>
                                            <thead>
                                                <tr>
                                                <th>Name</th>
                                                <th>Value</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td>xmp:CreateDate</td>
                                                    <td>2016-05-20T07:30:20Z</td>
                                                </tr>
                                                <tr>
                                                    <td>xmp:ModifyDate</td>
                                                    <td>2016-05-20T07:30:20Z</td>
                                                </tr>
                                                <tr>
                                                    <td>xmpDM:duration/xmpDM:value</td>
                                                    <td>33753</td>
                                                </tr>
                                                <tr>
                                                    <td>xmpDM:duration/xmpDM:scale</td>
                                                    <td>1/1000</td>
                                                </tr>
                                            </tbody>
                                        </Table>
                                    </Tab>
                                    <Tab eventKey={2} title="Transcoding">
                                        <Button bsStyle="primary">New task</Button>
                                        <Table striped bordered condensed hover>
                                            <thead>
                                                <tr>
                                                <th>Created At</th>
                                                <th>Started At</th>
                                                <th>Format</th>
                                                <th>Status</th>
                                                <th>Actions</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td>2018-05-20T07:30:20Z</td>
                                                    <td>2018-05-20T07:30:20Z</td>
                                                    <td>avi</td>
                                                    <td>Success</td>
                                                    <td><Button bsStyle="info">Info</Button></td>
                                                </tr>
                                                <tr>
                                                    <td>2018-05-20T07:30:20Z</td>
                                                    <td>2018-05-20T07:30:20Z</td>
                                                    <td>mp4</td>
                                                    <td>Success</td>
                                                    <td><Button bsStyle="info">Info</Button></td>
                                                </tr>
                                            </tbody>
                                        </Table>
                                    </Tab>
                                    <Tab eventKey={3} title="Quality Control">
                                        Tab 2 content
                                    </Tab>
                                    <Tab eventKey={4} title="Analytics">
                                        Tab 3 content
                                    </Tab>
                                    <Tab eventKey={5} title="Files">
                                        Tab 3 content
                                    </Tab>
                                </Tabs>;
                            </td>
                    </tr>
                    </tbody>
                </Table>
            </div>
        );
    }
}

export default Folders;