import { DateFormatter } from "@internationalized/date";

export const dateString = (date: string | undefined): string =>
  !date
    ? ""
    : new Date(date).toLocaleDateString("en-ZA", {
        year: "numeric",
        month: "short",
        day: "numeric",
      });

const df = new DateFormatter("en-ZA", {
  dateStyle: "long",
});

export const displayDate = (date: string | null | undefined): string =>
  date ? df.format(new Date(date)) : "Pick a date";
