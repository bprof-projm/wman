import React from 'react';
import { Page, Text, View, Document, StyleSheet } from '@react-pdf/renderer';
import './document-to-print.stlyes.css'
import { uuid } from 'uuidv4';



const DocumentToPrint = (props) => {


    const visible = (props.eventlist.length === 0);

    return (
        <Document>
            <Page size="A4">
                <View>
                    <div className="print-header">
                        <Text fixed>
                            -created by WMAN application-
                        </Text>
                    </div>
                    <div className='print-title'><h1>Events To Do:</h1></div>
                    <div className='print-all-events'>
                        <div className='one-event-top'>
                            <div className='one-event-title'>
                                <h2>Event Details:</h2>
                            </div>
                            <div className='one-event-notes'>
                                <h2>Notes:</h2>
                            </div>
                        </div>
                        <ol>
                            {props.eventlist?.map(event =>
                                <li key={uuid()}>
                                    <div className='one-event'>
                                        <br />
                                        <b>Desription: </b>{event.jobDescription}<br />
                                        <b>Start: </b>{event.estimatedStartDate}<br />
                                        <b>Finish: </b>{event.estimatedFinishDate}
                                        <h3>Address: </h3>
                                        <b>City: </b>{event.address.city}<br />
                                        <b>Street: </b>{event.address.street}<br />
                                        <b>ZipCode: </b>{event.address.zipCode}<br />
                                        <b>Building number: </b>{event.address.buildingNumber}<br />
                                        <b>Floor door: </b>{event.address.floorDoor}<br />
                                    </div>
                                    <hr />
                                </li>
                            )}
                        </ol>
                        <div className='preview-error'>
                            {visible ? <h3>There are no events assigned to the chosen user or you didn't choose anybody!</h3> : null}
                        </div>
                    </div>

                    <div className='pagenumber'>

                        <Text render={({ pageNumber, totalPages }) => (
                            `${pageNumber} / ${totalPages}`
                        )} fixed />
                    </div>
                </View>
            </Page>
        </Document>
    )
};

export default DocumentToPrint;