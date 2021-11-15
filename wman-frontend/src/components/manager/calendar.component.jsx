import React, { Component } from 'react';
import { Col, Row } from 'reactstrap';
import FullCalendar from '@fullcalendar/react';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import interactionPlugin, { Draggable } from '@fullcalendar/interaction';
import Alert from 'sweetalert2';
import '@fullcalendar/core/main.css';
import '@fullcalendar/daygrid/main.css';
import '@fullcalendar/timegrid/main.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import './calendar.styles.css';
import axios from 'axios';
import { Logout } from '../logout/logout.component';
import { MyEvent } from '../event/event.component';

class MyCalendar extends Component {
  state = {
    calendarEvents: [
      {
        description: 'Atlanta Monster2',
        start: new Date('2021-10-04 00:00'),
        title: 'asd',
        end: new Date('2021-10-06 00:00'),
        id: '99999998',
      },
    ],
    events: [
      {
        description: 'Atlanta Monster2',
        title: 'asd',
        start: new Date('2021-10-04 00:00'),
        end: new Date('2021-10-06 00:00'),
        id: '99999998',
      },
      { description: 'Event 2', title: 'asd', id: '2' },
      { description: 'Event 3', title: 'asd', id: '3' },
    ],
    key: '',
    eventTitle: '',
  };

  /**
   * adding dragable properties to external events through javascript
   */
  getEvents = () => {
    const minTime = new Promise((resolve) => setTimeout(resolve, 200));
    const req = axios.get(
      `https://mocki.io/v1/910b86b5-c17d-4b7f-a010-ef68461de47e`
    );

    Promise.all([minTime, req]).then((values) => {
      const reqData = values[1];
      console.log(reqData.data);
      this.setState({ events: reqData.data.events });
    });
  };
  componentDidMount() {
    let draggableEl = document.getElementById('external-events');
    new Draggable(draggableEl, {
      itemSelector: '.fc-event',
      eventData: function (eventEl) {
        let title = eventEl.getAttribute('title');
        let id = eventEl.getAttribute('data');

        console.log(eventEl);
        return {
          title: title,
          Id: id,
        };
      },
    });
    this.getEvents();
  }

  /**
   * when we click on event we are displaying event details
   */
  eventClick = (eventClick) => {
    console.log(eventClick);
    /*return(
      <MyEvent
      key={eventClick.event.Id}
      object={eventClick.event}
    />)      */
  };

  render() {
    return (
      <div className="animated fadeIn p-4 demo-app">
        <Row>
          <Col lg={3} sm={3} md={3}>
            <div
              id="external-events"
              style={{
                padding: '10px',
                width: '80%',
                height: 'auto',
                maxHeight: '-webkit-fill-available',
              }}
            >
              <p align="center">
                <strong>Events</strong>
              </p>
              {this.state.events.map((event) => (
                <div
                  className="fc-event"
                  title={event.JobDescription}
                  data={event.Id}
                  key={event.Id}
                >
                  {event.JobDescription}
                </div>
              ))}
            </div>
          </Col>

          <Col lg={9} sm={9} md={9}>
            <div className="demo-app-calendar" id="mycalendartest">
              <FullCalendar
                defaultView="dayGridMonth"
                header={{
                  left: 'prev,next today',
                  center: 'title',
                  right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek',
                }}
                rerenderDelay={10}
                eventDurationEditable={false}
                editable={true}
                droppable={true}
                plugins={[dayGridPlugin, timeGridPlugin, interactionPlugin]}
                ref={this.calendarComponentRef}
                weekends={this.state.calendarWeekends}
                events={this.state.calendarEvents}
                eventDrop={this.drop}
                // drop={this.drop}
                eventReceive={this.eventReceive}
                eventClick={this.eventClick}
                // selectable={true}
              />
            </div>
          </Col>
        </Row>
      </div>
    );
  }
}
export default MyCalendar;
