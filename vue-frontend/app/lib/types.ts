import type { FunctionalComponent } from "vue";

export type DetailItem = {
  heading: string;
  content: string | FunctionalComponent | Component;
};
