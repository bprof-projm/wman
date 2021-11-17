import React from 'react';
import { Page, Text, View, Document, StyleSheet } from '@react-pdf/renderer';
import './document-to-print.stlyes.css'

const DocumentToPrint = (props) => (

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
                    {console.log(props)}
                    Event1:<br/>
                    event date<br/>
                    event description<br/>
                    <br/>
                    Event2:<br/>
                    event date<br/>
                    event description<br/>
                    <br/>
                    Event3:<br/>
                    event date<br/>
                    event description 

                {props.eventlist.map(event =>
                    <div className='one-event'>
                        <p>Event Details</p>
                        <p>Desription:{event.jobDescription}</p>
                        <p>Start:{event.estimatedStartDate}</p>
                        <p>Finish:{event.estimatedFinishDate}</p>
                        <p>Address:</p>
                        <p>City:{event.address.city}</p>
                        <p>Street:{event.address.street}</p>
                        <p>ZipCode:{event.address.zipCode}</p>
                        <p>Building number:{event.address.buildingNumber}</p>
                        <p>Floor door:{event.address.floorDoor}</p>                        
                    </div>)}
                </div>

                <div className='pagenumber'>

                    <Text render={({ pageNumber, totalPages }) => (
                        `${pageNumber} / ${totalPages}`
                    )} fixed />
                </div>


            </View>
        </Page>
    </Document>
);

export default DocumentToPrint;